using System.Collections.Generic;
using Pixel.Attendance.Operation.Dtos;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Operation.Exporting
{
    public interface IEmployeeAbsencesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeAbsenceForViewDto> employeeAbsences);
    }
}