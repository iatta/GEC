using System.Collections.Generic;
using Pixel.GEC.Attendance.Attendance.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Attendance.Exporting
{
    public interface IMobileTransactionsExcelExporter
    {
        FileDto ExportToFile(List<GetMobileTransactionForViewDto> mobileTransactions);
    }
}