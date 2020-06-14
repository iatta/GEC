using System.Collections.Generic;
using GEC.Attendance.Attendance.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Attendance.Exporting
{
    public interface IUserDevicesExcelExporter
    {
        FileDto ExportToFile(List<GetUserDeviceForViewDto> userDevices);
    }
}