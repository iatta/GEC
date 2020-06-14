using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using GEC.Attendance.DataExporting.Excel.EpPlus;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;
using GEC.Attendance.Storage;

namespace GEC.Attendance.Setting.Exporting
{
    public class LocationCredentialsExcelExporter : EpPlusExcelExporterBase, ILocationCredentialsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocationCredentialsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocationCredentialForViewDto> locationCredentials)
        {
            return CreateExcelPackage(
                "LocationCredentials.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LocationCredentials"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Longitude"),
                        L("Latitude"),
                        (L("Location")) + L("DescriptionAr")
                        );

                    AddObjects(
                        sheet, 2, locationCredentials,
                        _ => _.LocationCredential.Longitude,
                        _ => _.LocationCredential.Latitude,
                        _ => _.LocationDescriptionAr
                        );

					

                });
        }
    }
}
