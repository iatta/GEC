﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Notifications;
using Abp.Organizations;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pixel.Attendance.Authorization.Permissions;
using Pixel.Attendance.Authorization.Permissions.Dto;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Authorization.Users.Dto;
using Pixel.Attendance.Authorization.Users.Exporting;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Notifications;
using Pixel.Attendance.Url;
using Pixel.Attendance.Organizations.Dto;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Operations.Dtos;
using Abp.Domain.Uow;
using Pixel.Attendance.Setting;
using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.Data.SqlClient;
using Abp.EntityFrameworkCore;
using System.Data.Common;
using System.Data;
using Abp.Data;
using Abp.MultiTenancy;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Pixel.Attendance.Helper;
using Pixel.Attendance.Setting.Dtos;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Pixel.Attendance.Configuration;

namespace Pixel.Attendance.Authorization.Users
{
    [AbpAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UserAppService : AttendanceAppServiceBase, IUserAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        private readonly IHttpClientFactory _clientFactory;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly IUserListExcelExporter _userListExcelExporter;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<RolePermissionSetting, long> _rolePermissionRepository;
        private readonly IRepository<UserPermissionSetting, long> _userPermissionRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserPolicy _userPolicy;
        private readonly IEnumerable<IPasswordValidator<User>> _passwordValidators;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRoleManagementConfig _roleManagementConfig;
        private readonly UserManager _userManager;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<TimeProfile> _timeProfileRepository;
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<TimeProfileDetail> _timeProfileDetailRepository;
        private readonly IRepository<JobTitle> _jobTitleRepository;
        private readonly IRepository<Nationality> _nationalityRepository;
        private readonly IActiveTransactionProvider _transactionProvider;
        private readonly IRepository<UserShift> _userShiftRepository;
        private readonly IRepository<OverrideShift> _overrideShiftRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Shift> _shiftRepository;
        private readonly IRepository<Beacon> _beaconRepository;
        private readonly IRepository<UserDelegation> _userDelegationRepository;
        private readonly IRepository<Machine> _machineRepository;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IDbContextProvider<AttendanceDbContext> _dbCOntext;

        public UserAppService(
            IAppConfigurationAccessor configurationAccessor,
            RoleManager roleManager,
            IHttpClientFactory clientFactory,
            IUserEmailer userEmailer,
            IUserListExcelExporter userListExcelExporter,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IRepository<RolePermissionSetting, long> rolePermissionRepository,
            IRepository<UserPermissionSetting, long> userPermissionRepository,
            IRepository<UserRole, long> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<OverrideShift> overrideShiftRepository,
            IUserPolicy userPolicy,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            IPasswordHasher<User> passwordHasher,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRoleManagementConfig roleManagementConfig,
            UserManager userManager,
            IRepository<Machine> machineRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<TimeProfile> timeProfileRepository,
            IRepository<JobTitle> jobTitleRepository,
            IRepository<Location> locationRepository,
            IRepository<TimeProfileDetail> timeProfileDetailRepository,
            IDbContextProvider<AttendanceDbContext> dbCOntext,
            IRepository<Nationality> nationalityRepository,
            IRepository<User, long> userRepository,
            IRepository<UserShift> userShift,
            IRepository<Shift> shift,
            IRepository<Beacon> beaconRepository,
            IActiveTransactionProvider transactionProvider,
            IRepository<UserDelegation> userDelegationRepository)
        {
            _roleManager = roleManager;
            _userEmailer = userEmailer;
            _overrideShiftRepository = overrideShiftRepository;
            _userListExcelExporter = userListExcelExporter;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _userRoleRepository = userRoleRepository;
            _userPolicy = userPolicy;
            _passwordValidators = passwordValidators;
            _passwordHasher = passwordHasher;
            _organizationUnitRepository = organizationUnitRepository;
            _roleManagementConfig = roleManagementConfig;
            _userManager = userManager;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            AppUrlService = NullAppUrlService.Instance;
            _timeProfileRepository = timeProfileRepository;
            _locationRepository = locationRepository;
            _timeProfileDetailRepository = timeProfileDetailRepository;
            _dbCOntext = dbCOntext;
            _transactionProvider = transactionProvider;
            _jobTitleRepository = jobTitleRepository;
            _nationalityRepository = nationalityRepository;
            _shiftRepository = shift;
            _userShiftRepository = userShift;
            _beaconRepository = beaconRepository;
            _userDelegationRepository = userDelegationRepository;
            _machineRepository = machineRepository;
            _clientFactory = clientFactory;
            _appConfiguration = configurationAccessor.Configuration;
        }

        public async Task<PagedResultDto<UserListDto>> GetUsers(GetUsersInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            foreach (var user in userListDtos)
            {
                var org = _organizationUnitRepository.FirstOrDefault(x => x.Id == user.OrganizationUnitId);
                if (org != null)
                    user.UnitName = org.DisplayName;

                var jobTitle = _jobTitleRepository.FirstOrDefault(x => x.Id == user.JobTitleId);
                if (jobTitle != null)
                    user.JobTitleName = jobTitle.NameAr;


            }
            await FillRoleNames(userListDtos);

            return new PagedResultDto<UserListDto>(
                userCount,
                userListDtos
                );
        }

        public async Task<PagedResultDto<DelegatedUserListDto>> GetDelegatedUsers(GetUsersInput input)
        {
            var currentUserId = GetCurrentUser().Id;
            var query = from user in _userDelegationRepository.GetAll()
                    join ur in _userRepository.GetAll() on user.UserId equals ur.Id into urJoined
                    from ur in urJoined.DefaultIfEmpty()
                    where user.DelegatedUserId == currentUserId
                        select new DelegatedUserListDto()
                        {
                            Id = ur.Id,
                            Name = ur.Name,
                            UserName = ur.UserName,
                            Surname = ur.Surname,
                            FromDate = user.FromDate,
                            ToDate = user.ToDate
                        };



            //var query = _userDelegationRepository.GetAll().Where(x => x.DelegatedUserId == GetCurrentUser().Id).Select(x => x.DelegatedUserFk);

            var userCount = await query.CountAsync();

            var users = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            //var userListDtos = ObjectMapper.Map<List<DelegatedUserListDto>>(users);

            return new PagedResultDto<DelegatedUserListDto>(
                userCount,
                users
                );
        }

        public async Task<List<UserListDto>> GetUsersFlat()
        {
            var users = await UserManager.Users.ToListAsync();
            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            return new List<UserListDto>(userListDtos);
        }


        public async Task<List<UserListDto>> GetUsersFlatByUnitId(int unitId)
        {
            var users2 = from user in UserManager.Users
                         where user.OrganizationUnitId == unitId
                         select user;
            var users =await  users2.ToListAsync();
            
            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            return new List<UserListDto>(userListDtos);
        }

        [HttpPost]
        public async Task UpdateUserFaceId(UpdateUserFaceIdInput input)
        {
            
            var user = UserManager.GetUserById(input.UserId);
            user.Image = input.Image;
            user.FaceMap = input.FaceMap;
            user.IsFaceRegistered = true;

            await UserManager.UpdateAsync(user);

            
        }

        public async Task<GetUserForFaceIdOutput> GetUserForFaceId(string civilId)
        {
            var user = UserManager.Users.Where(c => c.CivilId == civilId).FirstOrDefault();
            if (user == null)
                throw new UserFriendlyException("User Not Exist");

            var output =  ObjectMapper.Map<GetUserForFaceIdOutput>(user);
            if (output.OrganizationUnitId != null)
            {
                var unit = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.Id == output.OrganizationUnitId);
                output.UnitDisplayName = unit.DisplayName;
            }

            if (output.ManagerId != null)
            {
                output.ManagerDisplayName = _userManager.Users.FirstOrDefault(x => x.Id == output.ManagerId).Name;
            }

            return output;
        }

        public async Task<GetUserForEditOutput> GetExistUSerForActive(string civilId)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                //Editing an existing user
                var user = UserManager.Users.Where(c => c.CivilId == civilId && c.IsDeleted == true).FirstOrDefault();

                 //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();
                var allShifts = await _shiftRepository.GetAllListAsync();

            var output = new GetUserForEditOutput
            {
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
                MemberedOrganizationUnits = new List<string>(),
                MemberOrganizationUnit = "",
                Shifts = ObjectMapper.Map<List<ShiftDto>>(allShifts)
            };

            if (user != null)
            {
               

                output.User = ObjectMapper.Map<UserEditDto>(user);
                var timeProfile = _timeProfileRepository.GetAll().Where(x => x.UserId == output.User.Id).FirstOrDefault();
                output.ProfilePictureId = user.ProfilePictureId;


                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();



                output.MemberOrganizationUnit = allOrganizationUnits.Where(ou => ou.Id == user.OrganizationUnitId).Select(ou => ou.Code).FirstOrDefault();

                var allRolesOfUsersOrganizationUnits = GetAllRoleNamesOfUsersOrganizationUnits(user.Id);

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                    userRoleDto.InheritedFromOrganizationUnit = allRolesOfUsersOrganizationUnits.Contains(userRoleDto.RoleName);
                }
            }
           

            return output;
            }
               
                
           
        }

        public async Task<FileDto> GetUsersToExcel(GetUsersToExcelInput input)
        {
            var query = GetUsersFilteredQuery(input);

            var users = await query
                .OrderBy(input.Sorting)
                .ToListAsync();

            var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
            await FillRoleNames(userListDtos);

            return _userListExcelExporter.ExportToFile(userListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create, AppPermissions.Pages_Administration_Users_Edit)]
        public async Task<GetUserForEditOutput> GetUserForEdit(NullableIdDto<long> input)
        {
            //Getting all available roles
            var userRoleDtos = await _roleManager.Roles
                .OrderBy(r => r.DisplayName)
                .Select(r => new UserRoleDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    RoleDisplayName = r.DisplayName
                })
                .ToArrayAsync();

            //Getting all available Locations
            var userLocationDtos = await _locationRepository.GetAll()
                .OrderBy(r => r.TitleAr)
                .Select(r => new UserLocationDto
                {
                    LocationId = r.Id,
                    LocationDisplayName = r.TitleAr,
                    
                    
                })
                .ToArrayAsync();

            var allOrganizationUnits = await _organizationUnitRepository.GetAllListAsync();
            var nationalities = await _nationalityRepository.GetAllListAsync();
            var allBeacons = await _beaconRepository.GetAllListAsync();
            var allShifts = await _shiftRepository.GetAllListAsync();
            //user shifts 


            var output = new GetUserForEditOutput
            {
                
                Locations = userLocationDtos,
                Shifts = ObjectMapper.Map<List<ShiftDto>>(allShifts),
                Nationalities = ObjectMapper.Map<List<NationalityDto>>(nationalities),
                Roles = userRoleDtos,
                AllOrganizationUnits = ObjectMapper.Map<List<OrganizationUnitDto>>(allOrganizationUnits),
                AllBeacons = ObjectMapper.Map<List<BeaconDto>>(allBeacons),
                MemberedOrganizationUnits = new List<string>(),
                MemberOrganizationUnit = ""
            };

            if (!input.Id.HasValue)
            {
                //Creating a new user
                output.User = new UserEditDto
                {
                    IsActive = true,
                    ShouldChangePasswordOnNextLogin = true,
                    IsTwoFactorEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                    IsLockoutEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled)
                };

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    var defaultUserRole = userRoleDtos.FirstOrDefault(ur => ur.RoleName == defaultRole.Name);
                    if (defaultUserRole != null)
                    {
                        defaultUserRole.IsAssigned = true;
                    }
                }
            }
            else
            {
                //Editing an existing user
                var user = await UserManager.Users.FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                output.User = ObjectMapper.Map<UserEditDto>(user);
                var userShifts = from o in _overrideShiftRepository.GetAll()
                                 join o2 in _shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()
                                 where o.UserId == input.Id

                                 select new GetOverrideShiftForViewDto()
                                 {
                                     OverrideShift = new OverrideShiftDto
                                     {
                                         Day = o.Day,
                                         Id = o.Id,
                                         UserId = o.UserId,
                                         ShiftId = o.ShiftId
                                     },
                                     ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
                                 };


                output.User.OverrideShifts = await userShifts.ToListAsync();

                //var userShifts = from o in _userShiftRepository.GetAll()
                //                 join o2 in _shiftRepository.GetAll() on o.ShiftId equals o2.Id into j2
                //                 from s2 in j2.DefaultIfEmpty()
                //                 where o.UserId == input.Id

                //                 select new GetUserShiftForViewDto()
                //                 {
                //                     UserShift = new UserShiftDto
                //                     {
                //                         Date = o.Date,
                //                         Id = o.Id,
                //                         UserId = o.UserId,
                //                         ShiftId = o.ShiftId
                //                     },
                //                     ShiftNameEn = s2 == null ? "" : s2.NameEn.ToString()
                //                 };


                //output.User.UserShifts = await userShifts.ToListAsync();

                output.ProfilePictureId = user.ProfilePictureId;


                var organizationUnits = await UserManager.GetOrganizationUnitsAsync(user);
                output.MemberedOrganizationUnits = organizationUnits.Select(ou => ou.Code).ToList();


                
                output.MemberOrganizationUnit = allOrganizationUnits.Where(ou => ou.Id == user.OrganizationUnitId).Select(ou => ou.Code).FirstOrDefault();

                var allRolesOfUsersOrganizationUnits = GetAllRoleNamesOfUsersOrganizationUnits(input.Id.Value);

                foreach (var userRoleDto in userRoleDtos)
                {
                    userRoleDto.IsAssigned = await UserManager.IsInRoleAsync(user, userRoleDto.RoleName);
                    userRoleDto.InheritedFromOrganizationUnit = allRolesOfUsersOrganizationUnits.Contains(userRoleDto.RoleName);
                }

                //foreach (var userLocationDto in userLocationDtos)
                //{
                //    userLocationDto.IsAssigned = user.Locations.Any(x => x.LocationId == userLocationDto.LocationId);
                //    if (userLocationDto.IsAssigned)
                //    {
                //        userLocationDto.FromDate = user.Locations.Where(x => x.LocationId == userLocationDto.LocationId).FirstOrDefault().FromDate;
                //        userLocationDto.ToDate = user.Locations.Where(x => x.LocationId == userLocationDto.LocationId).FirstOrDefault().ToDate;
                //    }
                        
                //}
            }

            return output;
        }

        private List<string> GetAllRoleNamesOfUsersOrganizationUnits(long userId)
        {
            return (from userOu in _userOrganizationUnitRepository.GetAll()
                    join roleOu in _organizationUnitRoleRepository.GetAll() on userOu.OrganizationUnitId equals roleOu
                        .OrganizationUnitId
                    join userOuRoles in _roleRepository.GetAll() on roleOu.RoleId equals userOuRoles.Id
                    where userOu.UserId == userId
                    select userOuRoles.Name).ToList();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task<GetUserPermissionsForEditOutput> GetUserPermissionsForEdit(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var permissions = PermissionManager.GetAllPermissions();
            var grantedPermissions = await UserManager.GetGrantedPermissionsAsync(user);

            return new GetUserPermissionsForEditOutput
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList()
            };
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task ResetUserSpecificPermissions(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            await UserManager.ResetAllPermissionsAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_ChangePermissions)]
        public async Task UpdateUserPermissions(UpdateUserPermissionsInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            var grantedPermissions = PermissionManager.GetPermissionsFromNamesByValidating(input.GrantedPermissionNames);
            await UserManager.SetGrantedPermissionsAsync(user, grantedPermissions);
        }

        public async Task CreateOrUpdateUser(CreateOrUpdateUserInput input)
        {
            if (input.User.Id.HasValue)
            {
                await UpdateUserAsync(input);
            }
            else
            {
                await CreateUserAsync(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Delete)]
        public async Task DeleteUser(EntityDto<long> input)
        {
            if (input.Id == AbpSession.GetUserId())
            {
                throw new UserFriendlyException(L("YouCanNotDeleteOwnAccount"));
            }
            var user = await UserManager.GetUserByIdAsync(input.Id);
            CheckErrors(await UserManager.DeleteAsync(user));
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Unlock)]
        public async Task UnlockUser(EntityDto<long> input)
        {
            var user = await UserManager.GetUserByIdAsync(input.Id);
            user.Unlock();
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Edit)]
        protected virtual async Task UpdateUserAsync(CreateOrUpdateUserInput input)
        {
            Debug.Assert(input.User.Id != null, "input.User.Id should be set.");
            if (input.UserLoaded)
                CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete);
                
            var user = await UserManager.Users.Include(x => x.Locations).FirstOrDefaultAsync(x => x.Id == input.User.Id.Value);

            if (input.UserLoaded)
                user.IsDeleted = false;


            //Update user properties
            ObjectMapper.Map(input.User, user); //Passwords is not mapped (see mapping configuration)


            //update locations
            //var oldLocations = new HashSet<UserLocation>(user.Locations.ToList());
            var newLocations = new HashSet<AssignedLocationDto>(input.AssignedLocations.ToList());

            //foreach (var location in oldLocations)
            //{
            //    if (!newLocations.Any(x => x.LocationId == location.LocationId))
            //    {
            //        user.Locations.Remove(location);
            //    }

            //}
            user.Locations.Clear();
            await UserManager.UpdateAsync(user);

            foreach (var item in newLocations)
            {
              user.Locations.Add(new UserLocation(user.Id, item.LocationId,item.FromDate,item.ToDate));
            }

            CheckErrors(await UserManager.UpdateAsync(user));

            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.MobilePassword = EnryptString(randomPassword);
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await UserManager.ChangePasswordAsync(user, input.User.Password));
                user.MobilePassword = EnryptString(input.User.Password);
            }

            //Update roles
            CheckErrors(await UserManager.SetRolesAsync(user, input.AssignedRoleNames));

        
            //update organization units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    input.User.Password
                );
            }

            //add user shifts here 
            foreach (var userShiftModel in input.User.OverrideShifts)
            {
                //check if new 
                if (userShiftModel.OverrideShift.IsNew)
                    await _overrideShiftRepository.InsertAsync(ObjectMapper.Map<OverrideShift>(userShiftModel.OverrideShift));
                               

                if (userShiftModel.OverrideShift.IsDeleted && userShiftModel.OverrideShift.Id > 0)
                    await _overrideShiftRepository.HardDeleteAsync(ObjectMapper.Map<OverrideShift>(userShiftModel.OverrideShift));

            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Create)]
        protected virtual async Task CreateUserAsync(CreateOrUpdateUserInput input)
        {
            if (AbpSession.TenantId.HasValue)
            {
                await _userPolicy.CheckMaxUserCountAsync(AbpSession.GetTenantId());
            }
            var fingerCode = Convert.ToInt32(input.User.FingerCode);
            var machineId = input.User.MachineId;
            var uploadUser = input.User.UploadUser;
            var userImage = input.User.UserImage;
            var user = ObjectMapper.Map<User>(input.User); //Passwords is not mapped (see mapping configuration)
            user.TenantId = AbpSession.TenantId;

            //Set password
            if (input.SetRandomPassword)
            {
                var randomPassword = await _userManager.CreateRandomPassword();
                user.MobilePassword = EnryptString(randomPassword);
                user.Password = _passwordHasher.HashPassword(user, randomPassword);
                input.User.Password = randomPassword;
            }
            else if (!input.User.Password.IsNullOrEmpty())
            {
                await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
                foreach (var validator in _passwordValidators)
                {
                    CheckErrors(await validator.ValidateAsync(UserManager, user, input.User.Password));
                }
                user.MobilePassword = EnryptString(input.User.Password);
                user.Password = _passwordHasher.HashPassword(user, input.User.Password);
            }

            user.ShouldChangePasswordOnNextLogin = input.User.ShouldChangePasswordOnNextLogin;

            //Assign roles
            user.Roles = new Collection<UserRole>();
            foreach (var roleName in input.AssignedRoleNames)
            {
                var role = await _roleManager.GetRoleByNameAsync(roleName);
                user.Roles.Add(new UserRole(AbpSession.TenantId, user.Id, role.Id));
            }

            //Assign Locations
            user.Locations = new Collection<UserLocation>();
            foreach (var assignedLocation in input.AssignedLocations)
            {
                var locationToAdd = await _locationRepository.FirstOrDefaultAsync(x => x.Id == assignedLocation.LocationId);
                user.Locations.Add(new UserLocation( user.Id, locationToAdd.Id,assignedLocation.FromDate , assignedLocation.ToDate));
            }

            CheckErrors(await UserManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);

            //Organization Units
            await UserManager.SetOrganizationUnitsAsync(user, input.OrganizationUnits.ToArray());

            //Send activation email
            if (input.SendActivationEmail)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(
                    user,
                    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId),
                    input.User.Password
                );
            }

            //add user shifts 
            //foreach (var userShiftModel in input.User.UserShifts)
            //{
            //    //add user shift 
            //    await _userShiftRepository.InsertAsync(ObjectMapper.Map<UserShift>(userShiftModel.UserShift));
            //}
            foreach (var userShiftModel in input.User.OverrideShifts)
            {
                //add user shift 
                await _overrideShiftRepository.InsertAsync(ObjectMapper.Map<OverrideShift>(userShiftModel.OverrideShift));
            }

            //add user to machine
            var userToUpload = new UploadMachineUserInput();
            userToUpload.Person = new Person();
            userToUpload.MachineData = new MachineData();
            userToUpload.Person.UserCode = fingerCode;
            var machine = await _machineRepository.FirstOrDefaultAsync(x => x.Id == machineId);
            userToUpload.MachineData.IP = machine.IpAddress;
            userToUpload.MachineData.SN = machine.SubNet;
            userToUpload.MachineData.Port = machine.Port;

            var inputJson = new StringContent(
                 System.Text.Json.JsonSerializer.Serialize(userToUpload, new System.Text.Json.JsonSerializerOptions()), System.Text.Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync( _appConfiguration["Machine:uploadUserAPI"], inputJson);
            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    await System.Text.Json.JsonSerializer.DeserializeAsync<string>(responseStream);
                }

                var downloadImageInput = new DownloadImageInput();
                var clearImage = userImage.Split(",").ToList<string>();
                downloadImageInput.Datas = Convert.FromBase64String(clearImage[1]);
                downloadImageInput.MachineData = userToUpload.MachineData;
                downloadImageInput.UserCode = userToUpload.Person.UserCode;
                await UploadImage(downloadImageInput);

            }

        }
        [HttpPost]
        public async Task<DownloadImageInput> UploadImage(DownloadImageInput input)
        {
            var output = new DownloadImageInput();
            input.Image = Convert.ToBase64String(input.Datas);
            var inputJson = new StringContent(System.Text.Json.JsonSerializer.Serialize(input, new System.Text.Json.JsonSerializerOptions()), System.Text.Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(_appConfiguration["Machine:uploadImageAPI"], inputJson);
            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    output = await System.Text.Json.JsonSerializer.DeserializeAsync<DownloadImageInput>(responseStream);
                }
            }

            return output;
        }

        private async Task FillRoleNames(IReadOnlyCollection<UserListDto> userListDtos)
        {
            /* This method is optimized to fill role names to given list. */
            var userIds = userListDtos.Select(u => u.Id);

            var userRoles = await _userRoleRepository.GetAll()
                .Where(userRole => userIds.Contains(userRole.UserId))
                .Select(userRole => userRole).ToListAsync();

            var distinctRoleIds = userRoles.Select(userRole => userRole.RoleId).Distinct();

            foreach (var user in userListDtos)
            {
                var rolesOfUser = userRoles.Where(userRole => userRole.UserId == user.Id).ToList();
                user.Roles = ObjectMapper.Map<List<UserListRoleDto>>(rolesOfUser);
            }

            var roleNames = new Dictionary<int, string>();
            foreach (var roleId in distinctRoleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId.ToString());
                if (role != null)
                {
                    roleNames[roleId] = role.DisplayName;
                }
            }

            foreach (var userListDto in userListDtos)
            {
                foreach (var userListRoleDto in userListDto.Roles)
                {
                    if (roleNames.ContainsKey(userListRoleDto.RoleId))
                    {
                        userListRoleDto.RoleName = roleNames[userListRoleDto.RoleId];
                    }
                }

                userListDto.Roles = userListDto.Roles.Where(r => r.RoleName != null).OrderBy(r => r.RoleName).ToList();
            }
        }


        private IQueryable<User> GetUsersFilteredQuery(IGetUsersInput input)
        {
            var query = UserManager.Users
                .WhereIf(input.Role.HasValue, u => u.Roles.Any(r => r.RoleId == input.Role.Value))
                .WhereIf(input.OnlyLockedUsers, u => u.LockoutEndDateUtc.HasValue && u.LockoutEndDateUtc.Value > DateTime.UtcNow)
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            if (input.Permissions != null && input.Permissions.Any(p => !p.IsNullOrWhiteSpace()))
            {
                var staticRoleNames = _roleManagementConfig.StaticRoles.Where(
                    r => r.GrantAllPermissionsByDefault &&
                         r.Side == AbpSession.MultiTenancySide
                ).Select(r => r.RoleName).ToList();

                input.Permissions = input.Permissions.Where(p => !string.IsNullOrEmpty(p)).ToList();

                query = from user in query
                        join ur in _userRoleRepository.GetAll() on user.Id equals ur.UserId into urJoined
                        from ur in urJoined.DefaultIfEmpty()
                        join urr in _roleRepository.GetAll() on ur.RoleId equals urr.Id into urrJoined
                        from urr in urrJoined.DefaultIfEmpty()
                        join up in _userPermissionRepository.GetAll()
                            .Where(userPermission => input.Permissions.Contains(userPermission.Name)) on user.Id equals up.UserId into upJoined
                        from up in upJoined.DefaultIfEmpty()
                        join rp in _rolePermissionRepository.GetAll()
                            .Where(rolePermission => input.Permissions.Contains(rolePermission.Name)) on
                            new { RoleId = ur == null ? 0 : ur.RoleId } equals new { rp.RoleId } into rpJoined
                        from rp in rpJoined.DefaultIfEmpty()
                        where (up != null && up.IsGranted) ||
                              (up == null && rp != null && rp.IsGranted) ||
                              (up == null && rp == null && staticRoleNames.Contains(urr.Name))
                        //group user by user into userGrouped
                        select user;
            }

            return query;
        }

        public async Task<NotificationResponseDto> SendNotification(NotificaionInput input)
        {
            // to be modified
           return await Task.Run(() => {
                return new NotificationResponseDto();
            });
        }

        #region Reports
        public async Task<List<UserReportDto>> GetUsersByShiftId(int shiftId)
        {
            var command = CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetEmployeeByShift";
            command.CommandType = CommandType.StoredProcedure;
            command.Transaction = (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs{{"ContextType", typeof(AttendanceDbContext) }, { "MultiTenancySide", MultiTenancySides.Tenant } });
            var param = new SqlParameter("@shiftId", shiftId);
            command.Parameters.Add(param);

            using (var dataReader = await command.ExecuteReaderAsync())
            {
                var result = new List<UserReportDto>();

                while (dataReader.Read())
                {
                    result.Add(new UserReportDto() { 
                        UserId = (int)(long)dataReader[0] , 
                        UserName = dataReader[1].ToString(),
                        ShiftName = dataReader[2].ToString(),
                        ShiftId = (int)dataReader[3],
                        StartDate = DateTime.Parse(dataReader[4].ToString()),
                        EndDate = DateTime.Parse(dataReader[5].ToString())
                    });;
                }

                return result;
            }

           
        }

        
        [HttpPost]
        public async Task<List<InOutReportOutput>> GenerateInOutReport(ReportInput input)
        {
            try
            {
                var formattedQuery = $"exec SP_InOut_Summary_ByDate '{input.FromDate.ToString("yyyy-MM-dd")}','{input.ToDate.ToString("yyyy-MM-dd")}','{string.Join(",", input.UserIds)}'";

                var result = await CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().InOutReportOutputCore.FromSqlRaw(formattedQuery).ToListAsync();
                var output =  ObjectMapper.Map<List<InOutReportOutput>>(result);
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<List<InOutReportOutput>> GenerateForgetInOutReport(ReportInput input)
        {
            try
            {
                var formattedQuery = $"exec SP_ForgetInOut_Rpt '{input.FromDate.ToString("yyyy-MM-dd")}','{input.ToDate.ToString("yyyy-MM-dd")}','{string.Join(",", input.UserIds)}'";

                var result = await CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().ForgetInOutCore.FromSqlRaw(formattedQuery).ToListAsync();
                var output = ObjectMapper.Map<List<InOutReportOutput>>(result);
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<List<PermitReportOutput>> GeneratePermitReport(ReportInput input)
        {
            try
            {
                var formattedQuery = $"exec SP_Permission_GetEmpDate '{input.FromDate.ToString("yyyy-MM-dd")}','{input.ToDate.ToString("yyyy-MM-dd")}','{string.Join(",", input.UserIds)}'";

                var result = await CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().PermitReportCore.FromSqlRaw(formattedQuery).ToListAsync();
                var output = ObjectMapper.Map<List<PermitReportOutput>>(result);
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<List<FingerReportOutput>> GenerateFingerReport(ReportInput input)
        {
            try
            {
                var formattedQuery = $"exec SP_Transaction_Rpt_Emp_Machine '{input.FromDate.ToString("yyyy-MM-dd")}','{input.ToDate.ToString("yyyy-MM-dd")}','{string.Join(",", input.UserIds)}' , '{GetCurrentUser().Id}',''";

                var result = await CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().FingerReportCore.FromSqlRaw(formattedQuery).ToListAsync();
                var output = ObjectMapper.Map<List<FingerReportOutput>>(result);
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<List<EmployeeReportOutput>> CalculateDaysReport(ReportInput input)
        {
            try
            {
                var formattedQuery = $"exec CalculateDays '{input.FromDate.ToString("yyyy-MM-dd")}','{input.ToDate.ToString("yyyy-MM-dd")}' , '{input.Type}','{input.DaysCount}'";

                var result = await CurrentUnitOfWork.GetDbContext<AttendanceDbContext>().EmployeeReportCore.FromSqlRaw(formattedQuery).ToListAsync();
                var output = ObjectMapper.Map<List<EmployeeReportOutput>>(result);
                return output;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        #endregion
        #region MobilePAsswordEncryotion

        public string EnryptString(string strEncrypted)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }
        #endregion
    }
}
