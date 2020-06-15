using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
    public interface IEmployeeVacationsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeVacationForViewDto>> GetAll(GetAllEmployeeVacationsInput input);

        Task<GetEmployeeVacationForViewDto> GetEmployeeVacationForView(int id);

		Task<GetEmployeeVacationForEditOutput> GetEmployeeVacationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeVacationDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeVacationsToExcel(GetAllEmployeeVacationsForExcelInput input);

		
		Task<PagedResultDto<EmployeeVacationUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmployeeVacationLeaveTypeLookupTableDto>> GetAllLeaveTypeForLookupTable(GetAllForLookupTableInput input);
		Task CreateFromExcel(List<CreateOrEditEmployeeVacationDto> input);


	}
}