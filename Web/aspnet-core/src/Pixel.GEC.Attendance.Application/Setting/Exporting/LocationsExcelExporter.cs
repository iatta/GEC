using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Setting.Exporting
{
    public class LocationsExcelExporter : EpPlusExcelExporterBase, ILocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocationForViewDto> locations)
        {
            return CreateExcelPackage(
                "Locations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Locations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TitleAr"),
                        L("TitleEn")
                        );

                    AddObjects(
                        sheet, 2, locations,
                        _ => _.Location.TitleAr,
                        _ => _.Location.TitleEn
                        );

					

                });
        }
    }
}
