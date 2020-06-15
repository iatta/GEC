using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting
{
    public interface ILeaveTypesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLeaveTypeForViewDto>> GetAll(GetAllLeaveTypesInput input);

        Task<GetLeaveTypeForViewDto> GetLeaveTypeForView(int id);

		Task<GetLeaveTypeForEditOutput> GetLeaveTypeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLeaveTypeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLeaveTypesToExcel(GetAllLeaveTypesForExcelInput input);

		
    }
}