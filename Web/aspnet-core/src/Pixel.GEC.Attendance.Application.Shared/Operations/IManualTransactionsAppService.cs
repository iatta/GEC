using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Operations
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
		
    }
}