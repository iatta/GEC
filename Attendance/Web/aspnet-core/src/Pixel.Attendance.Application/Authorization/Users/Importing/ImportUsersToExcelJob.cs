using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Authorization.Users.Importing.Dto;
using Pixel.Attendance.Extended;
using Pixel.Attendance.Notifications;
using Pixel.Attendance.Setting;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Authorization.Users.Importing
{
    public class ImportUsersToExcelJob : BackgroundJob<ImportUsersFromExcelJobArgs>, ITransientDependency
    {
        private readonly RoleManager _roleManager;
        private readonly IUserListExcelDataReader _userListExcelDataReader;
        private readonly IInvalidUserExporter _invalidUserExporter;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<OrganizationUnitExtended, long> _organizationUnitRepository;

        public UserManager UserManager { get; set; }

        public ImportUsersToExcelJob(
            RoleManager roleManager,
            IUserListExcelDataReader userListExcelDataReader,
            IInvalidUserExporter invalidUserExporter,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IAppNotifier appNotifier,
             IRepository<Shift> shiftRepository,
             IRepository<OrganizationUnitExtended, long> organizationUnitRepository,
             IRepository<User, long> userRepository,
        IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _roleManager = roleManager;
            _userRepository = userRepository;
            _userListExcelDataReader = userListExcelDataReader;
            _invalidUserExporter = invalidUserExporter;
            _userPolicy = userPolicy;
            _organizationUnitRepository = organizationUnitRepository;
            _shiftRepository = shiftRepository;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(AttendanceConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportUsersFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var users = GetUserListFromExcelOrNull(args);
                if (users == null || !users.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateUsers(args, users);
            }
        }

        private List<ImportUserDto> GetUserListFromExcelOrNull(ImportUsersFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _userListExcelDataReader.GetUsersFromExcel(file.Bytes);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CreateUsers(ImportUsersFromExcelJobArgs args, List<ImportUserDto> users)
        {
            var invalidUsers = new List<ImportUserDto>();

            foreach (var user in users)
            {
                if (user.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateUserAsync(user));
                    }
                    catch (UserFriendlyException exception)
                    {
                        user.Exception = exception.Message;
                        invalidUsers.Add(user);
                    }
                    catch (Exception exception)
                    {
                        user.Exception = exception.ToString();
                        invalidUsers.Add(user);
                    }
                }
                else
                {
                    invalidUsers.Add(user);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportUsersResultAsync(args, invalidUsers));
        }

        private async Task CreateUserAsync(ImportUserDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            if (tenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(tenantId.Value);
            }
          
               var  user = _objectMapper.Map<User>(input); //Passwords is not mapped (see mapping configuration)

            //shift 
            var shift = await _shiftRepository.FirstOrDefaultAsync(x => x.NameAr== input.ShiftName
            || x.NameEn == input.ShiftName ||
            x.Code == input.ShiftName);
            if (shift != null)
                user.ShiftId = shift.Id;

            //unit
            var unit = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.DisplayName == input.Department);
            if (unit != null)
                user.OrganizationUnitId = (int)unit.Id;

            user.Password = input.Password;
            user.TenantId = tenantId;

            if (!input.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(tenantId);
                foreach (var validator in _passwordValidators)
                {
                    (await validator.ValidateAsync(UserManager, user, input.Password)).CheckErrors();
                }

                user.Password = _passwordHasher.HashPassword(user, input.Password);
            }

            user.Roles = new List<UserRole>();
            var roleList = _roleManager.Roles.ToList();

            foreach (var roleName in input.AssignedRoleNames)
            {
                var correspondingRoleName = GetRoleNameFromDisplayName(roleName, roleList);
                var role = await _roleManager.GetRoleByNameAsync(correspondingRoleName);
                user.Roles.Add(new UserRole(tenantId, user.Id, role.Id));
            }

            var currentUser = await _userRepository.FirstOrDefaultAsync(x => x.CivilId == input.CivilId);
            if (currentUser != null)
            {
                currentUser.ShiftId = user.ShiftId;
                currentUser.OrganizationUnitId = user.OrganizationUnitId;
                currentUser.FingerCode = user.FingerCode;
                currentUser.Password = user.Password;

                await UserManager.UpdateAsync(currentUser);
                
            }
           
            else
                (await UserManager.CreateAsync(user)).CheckErrors();
        }

        private async Task ProcessImportUsersResultAsync(ImportUsersFromExcelJobArgs args, List<ImportUserDto> invalidUsers)
        {
            if (invalidUsers.Any())
            {
                var file = _invalidUserExporter.ExportToFile(invalidUsers);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllUsersSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportUsersFromExcelJobArgs args)
        {
            AsyncHelper.RunSync(() => _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToUserList"),
                Abp.Notifications.NotificationSeverity.Warn));
        }

        private string GetRoleNameFromDisplayName(string displayName, List<Role> roleList)
        {
            return roleList.FirstOrDefault(
                        r => r.DisplayName?.ToLowerInvariant() == displayName?.ToLowerInvariant()
                    )?.Name;
        }
    }
}
