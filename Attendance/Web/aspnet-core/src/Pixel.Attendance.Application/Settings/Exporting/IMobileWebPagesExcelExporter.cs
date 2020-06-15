using System.Collections.Generic;
using Pixel.Attendance.Settings.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Settings.Exporting
{
    public interface IMobileWebPagesExcelExporter
    {
        FileDto ExportToFile(List<GetMobileWebPageForViewDto> mobileWebPages);
    }
}