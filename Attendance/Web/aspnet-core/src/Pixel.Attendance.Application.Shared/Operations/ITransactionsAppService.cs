using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using System.Collections.Generic;

namespace Pixel.Attendance.Operations
{
    public interface ITransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTransactionForViewDto>> GetAll(GetAllTransactionsInput input);

        Task<GetTransactionForViewDto> GetTransactionForView(int id);

		Task<GetTransactionForEditOutput> GetTransactionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTransactionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTransactionsToExcel(GetAllTransactionsForExcelInput input);

        Task<EntityExistDto> TransactionExist(CreateOrEditTransactionDto input);
        Task BulkUpdateTransactions(List<GetTransactionForViewDto> input);
        Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForUnitManager(GetTransactionDto input);

        Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForProjectManager(GetTransactionDto input);
        Task<PagedResultDto<GetTransactionForViewDto>> GetAllTransactionForHr(GetTransactionDto input);
        Task UpdateSingleTransaction(GetTransactionForViewDto input);
        Task<ActualSummerizeTimeSheetOutput> GetActualSummerizeTimeSheet(ActualSummerizeInput input);
        Task<ActualSummerizeTimeSheetOutput> GetMangerUsersToApprove(ActualSummerizeInput input);
        Task UnitManagerToApprove(ProjectManagerApproveInput input);
        Task PojectManagerApprove(ProjectManagerApproveInput input);
        Task PojectManagerReject(ProjectManagerApproveInput input);
        Task<List<NormalOverTimeReportOutput>> GetNormalOverTime(NormalOverTimeReportInput input);

        Task<List<NormalOverTimeReportOutput>> GetFixedOverTime(NormalOverTimeReportInput input);

        Task<List<NormalOverTimeReportOutput>> GetDepartmentTransactions(UnitTransactionsReportInput input);




    }
}