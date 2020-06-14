using System.Collections.Generic;
using GEC.Attendance.Operation.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Operation.Exporting
{
    public interface IEmployeeAbsencesExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeAbsenceForViewDto> employeeAbsences);
    }
}