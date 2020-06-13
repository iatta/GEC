using System.Collections.Generic;
using Pixel.GEC.Attendance.Auditing.Dto;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
