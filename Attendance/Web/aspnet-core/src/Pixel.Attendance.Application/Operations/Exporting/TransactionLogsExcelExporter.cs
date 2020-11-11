using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class TransactionLogsExcelExporter : EpPlusExcelExporterBase, ITransactionLogsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionLogsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionLogForViewDto> transactionLogs)
        {
            return CreateExcelPackage(
                "TransactionLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TransactionLogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("OldValue"),
                        L("NewValue"),
                        (L("Transaction")) + L("Transaction_Date"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, transactionLogs,
                        _ => _.TransactionLog.OldValue,
                        _ => _.TransactionLog.NewValue,
                        _ => _.TransactionTransaction_Date,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
