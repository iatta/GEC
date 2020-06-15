using System.Collections.Generic;
using Pixel.Attendance.Operations.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operations.Exporting
{
    public interface IEmployeeOfficialTaskDetailsExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeOfficialTaskDetailForViewDto> employeeOfficialTaskDetails);
    }
}