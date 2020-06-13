using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public class TransExcelExporter : EpPlusExcelExporterBase, ITransExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTranForViewDto> trans)
        {
            return CreateExcelPackage(
                "Trans.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Trans"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Scan1"),
                        L("Scan2"),
                        L("Scan3"),
                        L("Scan4"),
                        L("Scan5"),
                        L("Scan6"),
                        L("Scan8"),
                        L("ScanLocation1"),
                        L("ScanLocation2"),
                        L("ScanLocation3"),
                        L("ScanLocation4"),
                        L("ScanLocation5"),
                        L("ScanLocation6"),
                        L("ScanLocation7"),
                        L("ScanLocation8"),
                        L("HasHoliday"),
                        L("HasVacation"),
                        L("HasOffDay"),
                        L("IsAbsent"),
                        L("LeaveCode"),
                        L("DesignationID"),
                        L("LeaveRemark"),
                        L("NoShifts"),
                        L("ShiftName"),
                        L("ScanManual1"),
                        L("ScanManual2"),
                        L("ScanManual3"),
                        L("ScanManual4"),
                        L("ScanManual5"),
                        L("ScanManual6"),
                        L("ScanManual7"),
                        L("ScanManual8"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, trans,
                        _ => _.Tran.Scan1,
                        _ => _.Tran.Scan2,
                        _ => _.Tran.Scan3,
                        _ => _.Tran.Scan4,
                        _ => _.Tran.Scan5,
                        _ => _.Tran.Scan6,
                        _ => _.Tran.Scan8,
                        _ => _.Tran.ScanLocation1,
                        _ => _.Tran.ScanLocation2,
                        _ => _.Tran.ScanLocation3,
                        _ => _.Tran.ScanLocation4,
                        _ => _.Tran.ScanLocation5,
                        _ => _.Tran.ScanLocation6,
                        _ => _.Tran.ScanLocation7,
                        _ => _.Tran.ScanLocation8,
                        _ => _.Tran.HasHoliday,
                        _ => _.Tran.HasVacation,
                        _ => _.Tran.HasOffDay,
                        _ => _.Tran.IsAbsent,
                        _ => _.Tran.LeaveCode,
                        _ => _.Tran.DesignationID,
                        _ => _.Tran.LeaveRemark,
                        _ => _.Tran.NoShifts,
                        _ => _.Tran.ShiftName,
                        _ => _.Tran.ScanManual1,
                        _ => _.Tran.ScanManual2,
                        _ => _.Tran.ScanManual3,
                        _ => _.Tran.ScanManual4,
                        _ => _.Tran.ScanManual5,
                        _ => _.Tran.ScanManual6,
                        _ => _.Tran.ScanManual7,
                        _ => _.Tran.ScanManual8,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
