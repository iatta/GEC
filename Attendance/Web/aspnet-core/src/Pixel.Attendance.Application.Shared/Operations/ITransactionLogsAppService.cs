using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
    public interface ITransactionLogsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTransactionLogForViewDto>> GetAll(GetAllTransactionLogsInput input);

        Task<GetTransactionLogForViewDto> GetTransactionLogForView(int id);

		Task<GetTransactionLogForEditOutput> GetTransactionLogForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTransactionLogDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTransactionLogsToExcel(GetAllTransactionLogsForExcelInput input);

		
		Task<PagedResultDto<TransactionLogTransactionLookupTableDto>> GetAllTransactionForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TransactionLogUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		Task<List<GetTransactionLogForViewDto>> GetTransactionLogByTransId(int inId, int outId);

	}
}