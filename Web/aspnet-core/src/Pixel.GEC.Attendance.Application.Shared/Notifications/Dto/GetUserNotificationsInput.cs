using System;
using Abp.Notifications;
using Pixel.GEC.Attendance.Dto;

namespace Pixel.GEC.Attendance.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}