using System.Collections.Generic;
using Pixel.Attendance.Editions;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.MultiTenancy.Payments;
using Pixel.Attendance.MultiTenancy.Payments.Dto;

namespace Pixel.Attendance.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
