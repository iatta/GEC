using System.Collections.Generic;
using Abp;
using Pixel.GEC.Attendance.Chat.Dto;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
