using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IEmployeeTempTransfersAppService : IApplicationService 
    {
        Task<PagedResultDto<GetEmployeeTempTransferForViewDto>> GetAll(GetAllEmployeeTempTransfersInput input);

        Task<GetEmployeeTempTransferForViewDto> GetEmployeeTempTransferForView(int id);

		Task<GetEmployeeTempTransferForEditOutput> GetEmployeeTempTransferForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditEmployeeTempTransferDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetEmployeeTempTransfersToExcel(GetAllEmployeeTempTransfersForExcelInput input);

		
		Task<PagedResultDto<EmployeeTempTransferOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<EmployeeTempTransferUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}