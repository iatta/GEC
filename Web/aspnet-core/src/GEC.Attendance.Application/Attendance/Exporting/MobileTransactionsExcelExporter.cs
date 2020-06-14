using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Attendance.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Attendance.Exporting
{
    public class MobileTransactionsExcelExporter : EpPlusExcelExporterBase, IMobileTransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MobileTransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMobileTransactionForViewDto> mobileTransactions)
        {
            return CreateExcelPackage(
                "MobileTransactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MobileTransactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SiteId"),
                        L("SiteName")
                        );

                    AddObjects(
                        sheet, 2, mobileTransactions,
                        _ => _.MobileTransaction.SiteId,
                        _ => _.MobileTransaction.SiteName
                        );

					

                });
        }
    }
}
