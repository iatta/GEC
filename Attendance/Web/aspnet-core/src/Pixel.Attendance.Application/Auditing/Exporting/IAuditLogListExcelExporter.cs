using System.Collections.Generic;
using Pixel.Attendance.Auditing.Dto;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
