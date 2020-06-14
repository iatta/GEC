using System.Collections.Generic;
using GEC.Attendance.Attendance.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Attendance.Exporting
{
    public interface IMobileTransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetMobileTransactionForViewDto> mobileTransactions);
    }
}