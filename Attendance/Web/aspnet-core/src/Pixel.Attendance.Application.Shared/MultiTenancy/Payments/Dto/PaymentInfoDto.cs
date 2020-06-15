using Pixel.Attendance.Editions.Dto;

namespace Pixel.Attendance.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < AttendanceConsts.MinimumUpgradePaymentAmount;
        }
    }
}
