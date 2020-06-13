using Pixel.GEC.Attendance.Editions;
using Pixel.GEC.Attendance.Editions.Dto;
using Pixel.GEC.Attendance.MultiTenancy.Payments;
using Pixel.GEC.Attendance.Security;
using Pixel.GEC.Attendance.MultiTenancy.Payments.Dto;

namespace Pixel.GEC.Attendance.Web.Models.TenantRegistration
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
