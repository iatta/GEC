using GEC.Attendance.Editions.Dto;

namespace GEC.Attendance.MultiTenancy.Payments.Dto
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
