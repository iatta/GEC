using System.Collections.Generic;
using Pixel.GEC.Attendance.Editions;
using Pixel.GEC.Attendance.Editions.Dto;
using Pixel.GEC.Attendance.MultiTenancy.Payments;
using Pixel.GEC.Attendance.MultiTenancy.Payments.Dto;

namespace Pixel.GEC.Attendance.Web.Models.Payment
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
