using System.Collections.Generic;
using Abp;
using Pixel.Attendance.Chat.Dto;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
