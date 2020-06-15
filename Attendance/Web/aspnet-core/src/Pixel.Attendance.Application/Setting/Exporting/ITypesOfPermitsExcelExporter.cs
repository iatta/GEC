using System.Collections.Generic;
using Pixel.Attendance.Setting.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Setting.Exporting
{
    public interface ITypesOfPermitsExcelExporter
    {
        FileDto ExportToFile(List<GetTypesOfPermitForViewDto> typesOfPermits);
    }
}