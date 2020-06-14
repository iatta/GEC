using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Operations
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