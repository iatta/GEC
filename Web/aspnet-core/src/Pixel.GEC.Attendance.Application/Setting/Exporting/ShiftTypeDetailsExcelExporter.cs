using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Setting.Exporting
{
    public class ShiftTypeDetailsExcelExporter : EpPlusExcelExporterBase, IShiftTypeDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ShiftTypeDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetShiftTypeDetailForViewDto> shiftTypeDetails)
        {
            return CreateExcelPackage(
                "ShiftTypeDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ShiftTypeDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("InTimeFirstScan"),
                        L("InTimeLastScan"),
                        L("OutTimeFirstScan"),
                        L("OutTimeLastScan"),
                        (L("ShiftType")) + L("DescriptionAr")
                        );

                    AddObjects(
                        sheet, 2, shiftTypeDetails,
                        _ => _.ShiftTypeDetail.InTimeFirstScan,
                        _ => _.ShiftTypeDetail.InTimeLastScan,
                        _ => _.ShiftTypeDetail.OutTimeFirstScan,
                        _ => _.ShiftTypeDetail.OutTimeLastScan,
                        _ => _.ShiftTypeDescriptionAr
                        );

					

                });
        }
    }
}
