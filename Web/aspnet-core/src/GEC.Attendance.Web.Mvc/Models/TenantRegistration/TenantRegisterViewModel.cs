using GEC.Attendance.Editions;
using GEC.Attendance.Editions.Dto;
using GEC.Attendance.MultiTenancy.Payments;
using GEC.Attendance.Security;
using GEC.Attendance.MultiTenancy.Payments.Dto;

namespace GEC.Attendance.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
