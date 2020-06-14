using System.Collections.Generic;
using GEC.Attendance.Operations.Dtos;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Operations.Exporting
{
    public interface ITransExcelExporter
    {
        FileDto ExportToFile(List<GetTranForViewDto> trans);
    }
}