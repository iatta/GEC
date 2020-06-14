using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Operation.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Operation
{
    public interface IEmployeeAbsencesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeAbsenceForViewDto>> GetAll(GetAllEmployeeAbsencesInput input);

        Task<GetEmployeeAbsenceForViewDto> GetEmployeeAbsenceForView(int id);

		Task<GetEmployeeAbsenceForEditOutput> GetEmployeeAbsenceForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeAbsenceDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeAbsencesToExcel(GetAllEmployeeAbsencesForExcelInput input);

		
		Task<PagedResultDto<EmployeeAbsenceUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}