﻿using Abp.Events.Bus;

namespace Pixel.GEC.Attendance.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}