using System.Collections.Generic;
using Pixel.GEC.Attendance.Setting.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Setting.Exporting
{
    public interface ISystemConfigurationsExcelExporter
    {
        FileDto ExportToFile(List<GetSystemConfigurationForViewDto> systemConfigurations);
    }
}