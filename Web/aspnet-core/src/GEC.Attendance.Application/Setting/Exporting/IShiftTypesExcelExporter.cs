using System.Collections.Generic;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting.Exporting
{
    public interface IShiftTypesExcelExporter
    {
        FileDto ExportToFile(List<GetShiftTypeForViewDto> shiftTypes);
    }
}