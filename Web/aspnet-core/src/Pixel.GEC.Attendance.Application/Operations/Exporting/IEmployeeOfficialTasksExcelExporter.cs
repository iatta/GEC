using System.Collections.Generic;
using Pixel.GEC.Attendance.Operations.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Operations.Exporting
{
    public interface IEmployeeOfficialTasksExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeOfficialTaskForViewDto> employeeOfficialTasks);
    }
}