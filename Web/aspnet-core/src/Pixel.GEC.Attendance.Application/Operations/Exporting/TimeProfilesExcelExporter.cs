using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public class TimeProfilesExcelExporter : EpPlusExcelExporterBase, ITimeProfilesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TimeProfilesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTimeProfileForViewDto> timeProfiles)
        {
            return CreateExcelPackage(
                "TimeProfiles.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TimeProfiles"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DescriptionAr"),
                        L("DescriptionEn"),
                        L("StartDate"),
                        L("EndDate"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, timeProfiles,
                        _ => _.TimeProfile.DescriptionAr,
                        _ => _.TimeProfile.DescriptionEn,
                        _ => _timeZoneConverter.Convert(_.TimeProfile.StartDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.TimeProfile.EndDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName
                        );

					var startDateColumn = sheet.Column(3);
                    startDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					startDateColumn.AutoFit();
					var endDateColumn = sheet.Column(4);
                    endDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					endDateColumn.AutoFit();
					

                });
        }
    }
}
