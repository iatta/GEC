using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Setting.Exporting
{
    public class OverrideShiftsExcelExporter : EpPlusExcelExporterBase, IOverrideShiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OverrideShiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOverrideShiftForViewDto> overrideShifts)
        {
            return CreateExcelPackage(
                "OverrideShifts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OverrideShifts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Day"),
                        (L("User")) + L("Name"),
                        (L("Shift")) + L("NameEn")
                        );

                    AddObjects(
                        sheet, 2, overrideShifts,
                        _ => _timeZoneConverter.Convert(_.OverrideShift.Day, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.ShiftNameEn
                        );

					var dayColumn = sheet.Column(1);
                    dayColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dayColumn.AutoFit();
					

                });
        }
    }
}
