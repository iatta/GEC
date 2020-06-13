using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Operations
{
    public interface IEmployeePermitsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeePermitForViewDto>> GetAll(GetAllEmployeePermitsInput input);

		Task<PagedResultDto<GetEmployeePermitForViewDto>> getAllForManager(GetAllEmployeePermitsInput input);


		Task<GetEmployeePermitForViewDto> GetEmployeePermitForView(int id);

		Task<GetEmployeePermitForEditOutput> GetEmployeePermitForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeePermitDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeePermitsToExcel(GetAllEmployeePermitsForExcelInput input);

		
		Task<PagedResultDto<EmployeePermitUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmployeePermitPermitLookupTableDto>> GetAllPermitForLookupTable(GetAllForLookupTableInput input);
		
    }
}