using System.Collections.Generic;
using GEC.Attendance.Settings.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Settings.Exporting
{
    public interface IMobileWebPagesExcelExporter
    {
        FileDto ExportToFile(List<GetMobileWebPageForViewDto> mobileWebPages);
    }
}