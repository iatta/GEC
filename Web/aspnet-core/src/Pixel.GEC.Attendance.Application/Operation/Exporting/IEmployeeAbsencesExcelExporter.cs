using System.Collections.Generic;
using Pixel.GEC.Attendance.Operation.Dtos;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Operation.Exporting
{
    public interface IEmployeeAbsencesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeAbsenceForViewDto> employeeAbsences);
    }
}