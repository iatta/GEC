using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Setting.Exporting
{
    public class LocationMachinesExcelExporter : EpPlusExcelExporterBase, ILocationMachinesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LocationMachinesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLocationMachineForViewDto> locationMachines)
        {
            return CreateExcelPackage(
                "LocationMachines.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("LocationMachines"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        (L("Location")) + L("TitleAr"),
                        (L("Machine")) + L("NameEn")
                        );

                    AddObjects(
                        sheet, 2, locationMachines,
                        _ => _.LocationTitleAr,
                        _ => _.MachineNameEn
                        );

					

                });
        }
    }
}
