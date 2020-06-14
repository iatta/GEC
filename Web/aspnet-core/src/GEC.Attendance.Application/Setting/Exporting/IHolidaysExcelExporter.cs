using System.Collections.Generic;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting.Exporting
{
    public interface IHolidaysExcelExporter
    {
        FileDto ExportToFile(List<GetHolidayForViewDto> holidays);
    }
}