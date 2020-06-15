using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Settings.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Settings.Exporting
{
    public class MobileWebPagesExcelExporter : EpPlusExcelExporterBase, IMobileWebPagesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MobileWebPagesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMobileWebPageForViewDto> mobileWebPages)
        {
            return CreateExcelPackage(
                "MobileWebPages.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("MobileWebPages"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Content")
                        );

                    AddObjects(
                        sheet, 2, mobileWebPages,
                        _ => _.MobileWebPage.Name,
                        _ => _.MobileWebPage.Content
                        );

					

                });
        }
    }
}
