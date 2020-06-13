using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting
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