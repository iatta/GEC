using System;
using Abp.Notifications;
using GEC.Attendance.Dto;

namespace GEC.Attendance.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}