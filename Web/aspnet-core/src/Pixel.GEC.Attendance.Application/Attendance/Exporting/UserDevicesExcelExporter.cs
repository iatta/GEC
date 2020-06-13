using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Pixel.GEC.Attendance.DataExporting.Excel.EpPlus;
using Pixel.GEC.Attendance.Attendance.Dtos;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Storage;

namespace Pixel.GEC.Attendance.Attendance.Exporting
{
    public class UserDevicesExcelExporter : EpPlusExcelExporterBase, IUserDevicesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserDevicesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserDeviceForViewDto> userDevices)
        {
            return CreateExcelPackage(
                "UserDevices.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("UserDevices"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DeviceSN"),
                        L("LastToken"),
                        L("IP"),
                        L("OS"),
                        L("AppVersion"),
                        L("CivilID"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, userDevices,
                        _ => _.UserDevice.DeviceSN,
                        _ => _.UserDevice.LastToken,
                        _ => _.UserDevice.IP,
                        _ => _.UserDevice.OS,
                        _ => _.UserDevice.AppVersion,
                        _ => _.UserDevice.CivilID,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
