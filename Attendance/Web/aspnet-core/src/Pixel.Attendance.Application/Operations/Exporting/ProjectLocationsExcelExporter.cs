using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class ProjectLocationsExcelExporter : EpPlusExcelExporterBase, IProjectLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ProjectLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetProjectLocationForViewDto> projectLocations)
        {
            return CreateExcelPackage(
                "ProjectLocations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("ProjectLocations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Project")) + L("NameEn"),
                        (L("Location")) + L("TitleEn")
                        );

                    AddObjects(
                        sheet, 2, projectLocations,
                        _ => _.ProjectNameEn,
                        _ => _.LocationTitleEn
                        );

					

                });
        }
    }
}
