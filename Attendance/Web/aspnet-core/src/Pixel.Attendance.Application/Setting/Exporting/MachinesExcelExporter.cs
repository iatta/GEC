using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.Attendance.DataExporting.Excel.EpPlus;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Storage;

namespace Pixel.Attendance.Setting.Exporting
{
    public class MachinesExcelExporter : EpPlusExcelExporterBase, IMachinesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public MachinesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetMachineForViewDto> machines)
        {
            return CreateExcelPackage(
                "Machines.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Machines"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("NameAr"),
                        L("NameEn"),
                        L("IpAddress"),
                        L("SubNet"),
                        L("Status"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, machines,
                        _ => _.Machine.NameAr,
                        _ => _.Machine.NameEn,
                        _ => _.Machine.IpAddress,
                        _ => _.Machine.SubNet,
                        _ => _.Machine.Status,
                        _ => _.OrganizationUnitDisplayName
                        );

					

                });
        }
    }
}
