using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Attendance.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Attendance
{
    public interface IUserDevicesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserDeviceForViewDto>> GetAll(GetAllUserDevicesInput input);

        Task<GetUserDeviceForViewDto> GetUserDeviceForView(int id);

		Task<GetUserDeviceForEditOutput> GetUserDeviceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditUserDeviceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetUserDevicesToExcel(GetAllUserDevicesForExcelInput input);

		
		Task<PagedResultDto<UserDeviceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
        Task<bool> IsExist(string civilid, string sn);


    }
}