using System.Collections.Generic;
using Pixel.Attendance.Attendance.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Attendance.Exporting
{
    public interface IMobileTransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetMobileTransactionForViewDto> mobileTransactions);
    }
}