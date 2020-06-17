using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class UserShiftsExcelExporter : EpPlusExcelExporterBase, IUserShiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserShiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserShiftForViewDto> userShifts)
        {
            return CreateExcelPackage(
                "UserShifts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserShifts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Date"),
                        (L("User")) + L("Name"),
                        (L("Shift")) + L("NameEn")
                        );

                    AddObjects(
                        sheet, 2, userShifts,
                        _ => _timeZoneConverter.Convert(_.UserShift.Date, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.ShiftNameEn
                        );

					var dateColumn = sheet.Column(1);
                    dateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					dateColumn.AutoFit();
					

                });
        }
    }
}
