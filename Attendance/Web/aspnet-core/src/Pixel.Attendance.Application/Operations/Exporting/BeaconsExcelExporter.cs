using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Operations.Exporting
{
    public class BeaconsExcelExporter : EpPlusExcelExporterBase, IBeaconsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BeaconsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBeaconForViewDto> beacons)
        {
            return CreateExcelPackage(
                "Beacons.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Beacons"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Uid"),
                        L("Minor"),
                        L("Major")
                        );

                    AddObjects(
                        sheet, 2, beacons,
                        _ => _.Beacon.Name,
                        _ => _.Beacon.Uid,
                        _ => _.Beacon.Minor,
                        _ => _.Beacon.Major
                        );

					

                });
        }
    }
}
