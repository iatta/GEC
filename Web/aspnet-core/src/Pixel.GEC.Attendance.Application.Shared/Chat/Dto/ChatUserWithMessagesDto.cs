using System.Collections.Generic;

namespace Pixel.GEC.Attendance.Chat.Dto
{
    public class ChatUserWithMessagesDto : ChatUserDto
    {
        public List<ChatMessageDto> Messages { get; set; }
    }
}