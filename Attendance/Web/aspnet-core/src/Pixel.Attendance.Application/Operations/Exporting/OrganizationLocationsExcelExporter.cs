using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class OrganizationLocationsExcelExporter : EpPlusExcelExporterBase, IOrganizationLocationsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public OrganizationLocationsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetOrganizationLocationForViewDto> organizationLocations)
        {
            return CreateExcelPackage(
                "OrganizationLocations.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("OrganizationLocations"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("OrganizationUnit")) + L("DisplayName"),
                        (L("Location")) + L("TitleEn")
                        );

                    AddObjects(
                        sheet, 2, organizationLocations,
                        _ => _.OrganizationUnitDisplayName,
                        _ => _.LocationTitleEn
                        );

					

                });
        }
    }
}
