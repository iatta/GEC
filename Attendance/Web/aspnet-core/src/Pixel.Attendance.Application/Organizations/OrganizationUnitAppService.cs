using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Pixel.Attendance.Authorization;
using Pixel.Attendance.Organizations.Dto;
using System.Linq.Dynamic.Core;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Extended;
using Pixel.Attendance.Authorization.Users;
using Twilio.Exceptions;
using System.Collections.Generic;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Setting;

namespace Pixel.Attendance.Organizations
{
    [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits)]
    public class OrganizationUnitAppService : AttendanceAppServiceBase, IOrganizationUnitAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnitExtended, long> _organizationUnitRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<OrganizationUnitRole, long> _organizationUnitRoleRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly RoleManager _roleManager;
        private readonly IRepository<OrganizationLocation> _organizationLocationRepository;
        private readonly IRepository<Location> _locationRepository;
        
        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<Location> locationRepository,
            IRepository<OrganizationLocation> organizationLocationRepository,
            IRepository<OrganizationUnitExtended, long> organizationUnitRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            RoleManager roleManager,
            IRepository<OrganizationUnitRole, long> organizationUnitRoleRepository,
            IRepository<User, long> userRepository )
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _roleManager = roleManager;
            _organizationUnitRoleRepository = organizationUnitRoleRepository;
            _userRepository = userRepository;
            _organizationLocationRepository = organizationLocationRepository;
            _locationRepository = locationRepository;
    }


        

        public async Task<ListResultDto<OrganizationUnitDto>> GetCurrentManagerUnits()
        {
            //
            var organizationUnits = new List<OrganizationUnitExtended>();
            var childUnits = new List<OrganizationUnitExtended>();

            //get current user 
            var user = GetCurrentUser();

            // check if user is manager
            var unitManger = await _organizationUnitRepository.FirstOrDefaultAsync(x => x.ManagerId == user.Id);

            if (unitManger != null)
            {
                // user is manger  
                // get all sub units 
               
                var allUnits = _organizationUnitRepository.GetAll().ToList();
                organizationUnits.Add(unitManger);
                childUnits = GetChildes(childUnits, unitManger, allUnits);
                organizationUnits.AddRange(childUnits);

            }

            //var organizationUnits = await _organizationUnitRepository.GetAllListAsync();

            var organizationUnitMemberCounts = await UserManager.Users.Where(x => x.OrganizationUnitId != null)
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedUsers => new
                {
                    organizationUnitId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            var organizationUnitRoleCounts = await _organizationUnitRoleRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedRoles => new
                {
                    organizationUnitId = groupedRoles.Key,
                    count = groupedRoles.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            return new ListResultDto<OrganizationUnitDto>(
                organizationUnits.Select(ou =>
                {
                    var organizationUnitDto = ObjectMapper.Map<OrganizationUnitDto>(ou);
                    organizationUnitDto.MemberCount = organizationUnitMemberCounts.ContainsKey((int)ou.Id) ? organizationUnitMemberCounts[(int)ou.Id] : 0;
                    organizationUnitDto.RoleCount = organizationUnitRoleCounts.ContainsKey(ou.Id) ? organizationUnitRoleCounts[ou.Id] : 0;
                    var manager = _userRepository.FirstOrDefault(x => x.Id == organizationUnitDto.ManagerId);
                    if (manager != null)
                        organizationUnitDto.ManagerName = manager.Name;

                    return organizationUnitDto;
                }).ToList());
        }

        public async Task<ListResultDto<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var organizationUnits = await _organizationUnitRepository.GetAllIncluding(x => x.Locations).ToListAsync();

            var organizationUnitMemberCounts = await UserManager.Users.Where(x => x.OrganizationUnitId != null)
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedUsers => new
                {
                    organizationUnitId = groupedUsers.Key,
                    count = groupedUsers.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            var organizationUnitRoleCounts = await _organizationUnitRoleRepository.GetAll()
                .GroupBy(x => x.OrganizationUnitId)
                .Select(groupedRoles => new
                {
                    organizationUnitId = groupedRoles.Key,
                    count = groupedRoles.Count()
                }).ToDictionaryAsync(x => x.organizationUnitId, y => y.count);

            var output = new ListResultDto<OrganizationUnitDto>(
                organizationUnits.Select(ou =>
                {
                    var organizationUnitDto = ObjectMapper.Map<OrganizationUnitDto>(ou);
                    organizationUnitDto.MemberCount = organizationUnitMemberCounts.ContainsKey((int)ou.Id) ? organizationUnitMemberCounts[(int)ou.Id] : 0;
                    organizationUnitDto.RoleCount = organizationUnitRoleCounts.ContainsKey(ou.Id) ? organizationUnitRoleCounts[ou.Id] : 0;
                    var manager = _userRepository.FirstOrDefault(x => x.Id == organizationUnitDto.ManagerId);
                    if (manager != null)
                        organizationUnitDto.ManagerName = manager.Name;

                    return organizationUnitDto;
                }).ToList());

            foreach (var item in output.Items)
            {
                foreach (var location in item.Locations)
                {
                    location.LocationName = _locationRepository.FirstOrDefault(x => x.Id == location.LocationId).TitleEn;
                }
            }
            return output;
        }

        public async Task<PagedResultDto<OrganizationUnitUserListDto>> GetOrganizationUnitUsers(GetOrganizationUnitUsersInput input)
        {
            
            var usersQuery = UserManager.Users.Where(x => x.OrganizationUnitId == input.Id);

            var totalCount = await usersQuery.CountAsync();
            var items = await usersQuery.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitUserListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitUserDto = ObjectMapper.Map<OrganizationUnitUserListDto>(item);
                    organizationUnitUserDto.AddedTime = item.CreationTime;
                    return organizationUnitUserDto;
                }).ToList());
        }

        public async Task<PagedResultDto<OrganizationUnitRoleListDto>> GetOrganizationUnitRoles(GetOrganizationUnitRolesInput input)
        {
            var query = from ouRole in _organizationUnitRoleRepository.GetAll()
                        join ou in _organizationUnitRepository.GetAll() on ouRole.OrganizationUnitId equals ou.Id
                        join role in _roleManager.Roles on ouRole.RoleId equals role.Id
                        where ouRole.OrganizationUnitId == input.Id
                        select new
                        {
                            ouRole,
                            role
                        };

            var totalCount = await query.CountAsync();
            var items = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<OrganizationUnitRoleListDto>(
                totalCount,
                items.Select(item =>
                {
                    var organizationUnitRoleDto = ObjectMapper.Map<OrganizationUnitRoleListDto>(item.role);
                    organizationUnitRoleDto.AddedTime = item.ouRole.CreationTime;
                    return organizationUnitRoleDto;
                }).ToList());
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnitExtended(AbpSession.TenantId, input.DisplayName, input.ParentId);
            organizationUnit.ManagerId = input.ManagerId;
            organizationUnit.HasApprove = input.HasApprove;
            organizationUnit.Locations = new List<OrganizationLocation>();
            if (input.Locations.Count > 0)
            {
                foreach (var item in input.Locations)
                {
                    organizationUnit.Locations.Add(ObjectMapper.Map<OrganizationLocation>(item));
                }

            }


            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);
            organizationUnit.ManagerId = input.ManagerId;
            organizationUnit.DisplayName = input.DisplayName;
            organizationUnit.HasApprove = input.HasApprove;
            organizationUnit.Locations = _organizationLocationRepository.GetAll().Where(x => x.OrganizationUnitId == organizationUnit.Id).ToList();

            var oldOrganizationLocations = new HashSet<OrganizationLocation>(organizationUnit.Locations.ToList());
            var newOrganizationLocations = new HashSet<OrganizationLocationDto>(input.Locations.ToList());

            foreach (var detail in oldOrganizationLocations)
            {
                if (!newOrganizationLocations.Any(x => x.Id == detail.Id))
                {
                    organizationUnit.Locations.Remove(detail);
                }
                else
                {
                    var inputDetail = newOrganizationLocations.Where(x => x.Id == detail.Id).FirstOrDefault();
                }

            }

            foreach (var item in newOrganizationLocations)
            {
                if (item.Id == 0)
                {
                    organizationUnit.Locations.Add(ObjectMapper.Map<OrganizationLocation>(item));
                }
            }

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
                );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree)]
        public async Task DeleteOrganizationUnit(EntityDto<long> input)
        {
            var count = await _organizationUnitRepository.CountAsync(c => c.ParentId == input.Id);
            if (count > 0)
                throw new ApiException("Can't Delete This Unit");
            else
            {
                var users = UserManager.Users.Where(x => x.OrganizationUnitId == input.Id).ToList();
                foreach (var user in users)
                {
                    user.OrganizationUnitId = null;
                    await UserManager.UpdateAsync(user);
                }
                await _organizationLocationRepository.DeleteAsync(x => x.OrganizationUnitId == input.Id);

                await _userOrganizationUnitRepository.DeleteAsync(x => x.OrganizationUnitId == input.Id);
                await _organizationUnitRoleRepository.DeleteAsync(x => x.OrganizationUnitId == input.Id);

                //check dependencies 
                await _organizationUnitRepository.HardDeleteAsync(_organizationUnitRepository.FirstOrDefault(x => x.Id == input.Id));
            }
          
        }


        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task RemoveUserFromOrganizationUnit(UserToOrganizationUnitInput input)
        {
            var user = UserManager.Users.Where(x => x.Id == input.UserId).FirstOrDefault();
            user.OrganizationUnitId = null;
            await UserManager.UpdateAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task RemoveRoleFromOrganizationUnit(RoleToOrganizationUnitInput input)
        {
            

            await _roleManager.RemoveFromOrganizationUnitAsync(input.RoleId, input.OrganizationUnitId);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task AddUsersToOrganizationUnit(UsersToOrganizationUnitInput input)
        {
            foreach (var userId in input.UserIds)
            {
                var user = UserManager.Users.Where(x => x.Id == userId).FirstOrDefault();
                user.OrganizationUnitId = input.OrganizationUnitId;
                await UserManager.UpdateAsync(user);

                //await UserManager.AddToOrganizationUnitAsync(userId, input.OrganizationUnitId);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task AddRolesToOrganizationUnit(RolesToOrganizationUnitInput input)
        {
            foreach (var roleId in input.RoleIds)
            {
                await _roleManager.AddToOrganizationUnitAsync(roleId, input.OrganizationUnitId, AbpSession.TenantId);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers)]
        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindOrganizationUnitUsersInput input)
        {
            var userIdsInOrganizationUnit = UserManager.Users
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.Id);

            var query = UserManager.Users
                .Where(u => !userIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter) ||
                        u.Surname.Contains(input.Filter) ||
                        u.UserName.Contains(input.Filter) ||
                        u.EmailAddress.Contains(input.Filter)
                );

            var userCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                userCount,
                users.Select(u =>
                    new NameValueDto(
                        u.FullName + " (" + u.EmailAddress + ")",
                        u.Id.ToString()
                    )
                ).ToList()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles)]
        public async Task<PagedResultDto<NameValueDto>> FindRoles(FindOrganizationUnitRolesInput input)
        {
            var roleIdsInOrganizationUnit = _organizationUnitRoleRepository.GetAll()
                .Where(uou => uou.OrganizationUnitId == input.OrganizationUnitId)
                .Select(uou => uou.RoleId);

            var query = _roleManager.Roles
                .Where(u => !roleIdsInOrganizationUnit.Contains(u.Id))
                .WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.DisplayName.Contains(input.Filter) ||
                        u.Name.Contains(input.Filter)
                );

            var roleCount = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.DisplayName)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<NameValueDto>(
                roleCount,
                users.Select(u =>
                    new NameValueDto(
                        u.DisplayName,
                        u.Id.ToString()
                    )
                ).ToList()
            );
        }

        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = ObjectMapper.Map<OrganizationUnitDto>(organizationUnit);
            dto.MemberCount = await _userOrganizationUnitRepository.CountAsync(uou => uou.OrganizationUnitId == organizationUnit.Id);
            return dto;
        }

        private List<OrganizationUnitExtended> GetChildes(List<OrganizationUnitExtended> childs, OrganizationUnitExtended unit, List<OrganizationUnitExtended> units)
        {
            if (unit.Children.Count > 0)
            {
                foreach (var child in unit.Children)
                {
                    childs.Add(ObjectMapper.Map<OrganizationUnitExtended>(child));
                    var newEntity = _organizationUnitRepository.GetAllIncluding(x => x.Children).FirstOrDefault(d => d.Id == child.Id);
                    if (newEntity.Children.Count > 0)
                    {
                        GetChildes(childs, newEntity, units);
                    }
                }

            }

            return childs;

        }
    }
}