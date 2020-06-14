using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Setting.Exporting
{
    public class ShiftsExcelExporter : EpPlusExcelExporterBase, IShiftsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftForViewDto> shifts)
        {
            return CreateExcelPackage(
                "Shifts.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Shifts"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NameAr"),
                        L("NameEn"),
                        L("Code"),
                        L("TimeIn"),
                        L("TimeOut"),
                        L("EarlyIn"),
                        L("LateIn"),
                        L("EarlyOut"),
                        L("LateOut"),
                        L("TimeInRangeFrom"),
                        L("TimeInRangeTo"),
                        L("TimeOutRangeFrom"),
                        L("TimeOutRangeTo")
                        );

                    AddObjects(
                        sheet, 2, shifts,
                        _ => _.Shift.NameAr,
                        _ => _.Shift.NameEn,
                        _ => _.Shift.Code,
                        _ => _.Shift.TimeIn,
                        _ => _.Shift.TimeOut,
                        _ => _.Shift.EarlyIn,
                        _ => _.Shift.LateIn,
                        _ => _.Shift.EarlyOut,
                        _ => _.Shift.LateOut,
                        _ => _.Shift.TimeInRangeFrom,
                        _ => _.Shift.TimeInRangeTo,
                        _ => _.Shift.TimeOutRangeFrom,
                        _ => _.Shift.TimeOutRangeTo
                        );

					

                });
        }
    }
}
