using Abp.Events.Bus;

namespace GEC.Attendance.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}