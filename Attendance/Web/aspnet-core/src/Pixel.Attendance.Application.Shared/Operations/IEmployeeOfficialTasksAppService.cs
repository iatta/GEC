using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IEmployeeOfficialTasksAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeOfficialTaskForViewDto>> GetAll(GetAllEmployeeOfficialTasksInput input);

        Task<GetEmployeeOfficialTaskForViewDto> GetEmployeeOfficialTaskForView(int id);

		Task<GetEmployeeOfficialTaskForEditOutput> GetEmployeeOfficialTaskForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeOfficialTaskDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeOfficialTasksToExcel(GetAllEmployeeOfficialTasksForExcelInput input);

		
		Task<PagedResultDto<EmployeeOfficialTaskOfficialTaskTypeLookupTableDto>> GetAllOfficialTaskTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}