using System.Collections.Generic;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Exporting
{
    public interface IShiftTypeDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetShiftTypeDetailForViewDto> shiftTypeDetails);
    }
}