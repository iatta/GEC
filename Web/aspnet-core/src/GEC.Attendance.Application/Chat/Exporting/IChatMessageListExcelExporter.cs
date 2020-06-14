using System.Collections.Generic;
using Abp;
using GEC.Attendance.Chat.Dto;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
