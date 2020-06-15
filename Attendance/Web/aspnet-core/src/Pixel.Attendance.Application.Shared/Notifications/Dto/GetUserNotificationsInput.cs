﻿using System;
using Abp.Notifications;
using Pixel.Attendance.Dto;

namespace Pixel.Attendance.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}