﻿namespace Pixel.GEC.Attendance.MultiTenancy.HostDashboard.Dto
{
    public class ExpiringTenant
    {
        public string TenantName { get; set; }
        public int RemainingDayCount { get; set; }
    }
}