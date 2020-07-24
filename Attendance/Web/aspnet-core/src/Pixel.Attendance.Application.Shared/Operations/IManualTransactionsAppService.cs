using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations
{
    public interface IManualTransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetManualTransactionForViewDto>> GetAll(GetAllManualTransactionsInput input);

        Task<GetManualTransactionForViewDto> GetManualTransactionForView(int id);

		Task<GetManualTransactionForEditOutput> GetManualTransactionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditManualTransactionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetManualTransactionsToExcel(GetAllManualTransactionsForExcelInput input);

		
		Task<PagedResultDto<ManualTransactionUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ManualTransactionMachineLookupTableDto>> GetAllMachineForLookupTable(GetAllForLookupTableInput input);
		
    }
}