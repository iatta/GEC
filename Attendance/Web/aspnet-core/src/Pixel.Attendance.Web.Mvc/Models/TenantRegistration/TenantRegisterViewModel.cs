using Pixel.Attendance.Editions;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.MultiTenancy.Payments;
using Pixel.Attendance.Security;
using Pixel.Attendance.MultiTenancy.Payments.Dto;

namespace Pixel.Attendance.Web.Models.TenantRegistration
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
