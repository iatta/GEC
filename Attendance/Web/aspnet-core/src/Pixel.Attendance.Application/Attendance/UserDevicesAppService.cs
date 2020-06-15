using Pixel.Attendance.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Pixel.Attendance.Attendance.Exporting;
using Pixel.Attendance.Attendance.Dtos;
using Pixel.Attendance.Dto;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Pixel.Attendance.Attendance
{
	[AbpAuthorize(AppPermissions.Pages_UserDevices)]
    public class UserDevicesAppService : AttendanceAppServiceBase, IUserDevicesAppService
    {
		 private readonly IRepository<UserDevice> _userDeviceRepository;
		 private readonly IUserDevicesExcelExporter _userDevicesExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public UserDevicesAppService(IRepository<UserDevice> userDeviceRepository, IUserDevicesExcelExporter userDevicesExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_userDeviceRepository = userDeviceRepository;
			_userDevicesExcelExporter = userDevicesExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserDeviceForViewDto>> GetAll(GetAllUserDevicesInput input)
         {
			
			var filteredUserDevices = _userDeviceRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DeviceSN.Contains(input.Filter) || e.LastToken.Contains(input.Filter) || e.IP.Contains(input.Filter) || e.OS.Contains(input.Filter) || e.AppVersion.Contains(input.Filter) || e.CivilID.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredUserDevices = filteredUserDevices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userDevices = from o in pagedAndFilteredUserDevices
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetUserDeviceForViewDto() {
							UserDevice = new UserDeviceDto
							{
                                DeviceSN = o.DeviceSN,
                                LastToken = o.LastToken,
                                IP = o.IP,
                                OS = o.OS,
                                AppVersion = o.AppVersion,
                                CivilID = o.CivilID,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredUserDevices.CountAsync();

            return new PagedResultDto<GetUserDeviceForViewDto>(
                totalCount,
                await userDevices.ToListAsync()
            );
         }
		 
		 public async Task<GetUserDeviceForViewDto> GetUserDeviceForView(int id)
         {
            var userDevice = await _userDeviceRepository.GetAsync(id);

            var output = new GetUserDeviceForViewDto { UserDevice = ObjectMapper.Map<UserDeviceDto>(userDevice) };

		    if (output.UserDevice.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDevice.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UserDevices_Edit)]
		 public async Task<GetUserDeviceForEditOutput> GetUserDeviceForEdit(EntityDto input)
         {
            var userDevice = await _userDeviceRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserDeviceForEditOutput {UserDevice = ObjectMapper.Map<CreateOrEditUserDeviceDto>(userDevice)};

		    if (output.UserDevice.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDevice.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }
        [AbpAllowAnonymous]
        public async Task CreateOrEdit(CreateOrEditUserDeviceDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

        //[AbpAuthorize(AppPermissions.Pages_UserDevices_Create)]
        [AbpAllowAnonymous]
        protected virtual async Task Create(CreateOrEditUserDeviceDto input)
         {
            var userDevice = ObjectMapper.Map<UserDevice>(input);

			

            await _userDeviceRepository.InsertAsync(userDevice);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDevices_Edit)]
		 protected virtual async Task Update(CreateOrEditUserDeviceDto input)
         {
            var userDevice = await _userDeviceRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, userDevice);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDevices_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _userDeviceRepository.DeleteAsync(input.Id);
         }
        [AbpAllowAnonymous]
        public async Task<bool> IsExist(string civilid,string sn)
        {
            var res = await _userDeviceRepository.GetAll().Where(x => x.CivilID == civilid & x.DeviceSN == sn).ToListAsync();
            return res.Count > 0;
        }
        public async Task<FileDto> GetUserDevicesToExcel(GetAllUserDevicesForExcelInput input)
         {
			
			var filteredUserDevices = _userDeviceRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.DeviceSN.Contains(input.Filter) || e.LastToken.Contains(input.Filter) || e.IP.Contains(input.Filter) || e.OS.Contains(input.Filter) || e.AppVersion.Contains(input.Filter) || e.CivilID.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredUserDevices
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetUserDeviceForViewDto() { 
							UserDevice = new UserDeviceDto
							{
                                DeviceSN = o.DeviceSN,
                                LastToken = o.LastToken,
                                IP = o.IP,
                                OS = o.OS,
                                AppVersion = o.AppVersion,
                                CivilID = o.CivilID,
                                Id = o.Id
							},
                         	UserName = s1 == null ? "" : s1.Name.ToString()
						 });


            var userDeviceListDtos = await query.ToListAsync();

            return _userDevicesExcelExporter.ExportToFile(userDeviceListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UserDevices)]
         public async Task<PagedResultDto<UserDeviceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDeviceUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserDeviceUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserDeviceUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}