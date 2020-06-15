using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IEmployeeWarningsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeWarningForViewDto>> GetAll(GetAllEmployeeWarningsInput input);

        Task<GetEmployeeWarningForViewDto> GetEmployeeWarningForView(int id);

		Task<GetEmployeeWarningForEditOutput> GetEmployeeWarningForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeWarningDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeWarningsToExcel(GetAllEmployeeWarningsForExcelInput input);

		
		Task<PagedResultDto<EmployeeWarningUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmployeeWarningWarningTypeLookupTableDto>> GetAllWarningTypeForLookupTable(GetAllForLookupTableInput input);
		
    }
}