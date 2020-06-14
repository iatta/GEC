using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Setting.Exporting
{
    public class ShiftTypesExcelExporter : EpPlusExcelExporterBase, IShiftTypesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftTypesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftTypeForViewDto> shiftTypes)
        {
            return CreateExcelPackage(
                "ShiftTypes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShiftTypes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DescriptionEn"),
                        L("DescriptionAr"),
                        L("NumberOfDuties"),
                        L("InScan"),
                        L("OutScan"),
                        L("CrossDay"),
                        L("AlwaysAttend"),
                        L("Open"),
                        L("MaxBoundryTime")
                        );

                    AddObjects(
                        sheet, 2, shiftTypes,
                        _ => _.ShiftType.DescriptionEn,
                        _ => _.ShiftType.DescriptionAr,
                        _ => _.ShiftType.NumberOfDuties,
                        _ => _.ShiftType.InScan,
                        _ => _.ShiftType.OutScan,
                        _ => _.ShiftType.CrossDay,
                        _ => _.ShiftType.AlwaysAttend,
                        _ => _.ShiftType.Open,
                        _ => _.ShiftType.MaxBoundryTime
                        );

					

                });
        }
    }
}
