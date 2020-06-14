using System.Collections.Generic;
using GEC.Attendance.Setting.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Setting.Exporting
{
    public interface IJobTitleExcelExporter
    {
        FileDto ExportToFile(List<GetJobTitleForViewDto> jobTitles);
    }
}