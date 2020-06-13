using System.Collections.Generic;
using Pixel.GEC.Attendance.Settings.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Settings.Exporting
{
    public interface IMobileWebPagesExcelExporter
    {
        FileDto ExportToFile(List<GetMobileWebPageForViewDto> mobileWebPages);
    }
}