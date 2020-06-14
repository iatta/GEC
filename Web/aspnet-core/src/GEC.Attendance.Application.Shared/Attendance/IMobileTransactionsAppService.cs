using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Attendance.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Attendance
{
    public interface IMobileTransactionsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetMobileTransactionForViewDto>> GetAll(GetAllMobileTransactionsInput input);

		Task<GetMobileTransactionForEditOutput> GetMobileTransactionForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditMobileTransactionDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetMobileTransactionsToExcel(GetAllMobileTransactionsForExcelInput input);
        Task<CheckEmpLocationOutputModel> CheckEmpLocation(CheckEmpLocationInputModel input);
        Task<InsertTransactionOutputModel> InsertTransaction(InsertTransactionInputModel input);
        Task<ReportOutputModel> Report(ReportInputModel input);
        Task<LastTransOutputModel> GetLastTransaction(LastTransactionInputModel input);
        Task<GetEmpLocationsOutputModel> GetEmpLocations( ReportInputModel input);
    }
}