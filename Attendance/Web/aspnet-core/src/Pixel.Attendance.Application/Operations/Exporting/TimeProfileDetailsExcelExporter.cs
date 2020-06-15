using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class TimeProfileDetailsExcelExporter : EpPlusExcelExporterBase, ITimeProfileDetailsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TimeProfileDetailsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTimeProfileDetailForViewDto> timeProfileDetails)
        {
            return CreateExcelPackage(
                "TimeProfileDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("TimeProfileDetails"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("TimeProfile")) + L("DescriptionAr"),
                        (L("Shift")) + L("NameAr")
                        );

                    AddObjects(
                        sheet, 2, timeProfileDetails,
                        _ => _.TimeProfileDescriptionAr,
                        _ => _.ShiftNameAr
                        );

					

                });
        }
    }
}
