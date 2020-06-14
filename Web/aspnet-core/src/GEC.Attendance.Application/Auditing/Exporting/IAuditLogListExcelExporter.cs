using System.Collections.Generic;
using GEC.Attendance.Auditing.Dto;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
