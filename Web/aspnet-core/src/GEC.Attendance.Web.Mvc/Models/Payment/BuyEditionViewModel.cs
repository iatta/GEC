using System.Collections.Generic;
using GEC.Attendance.Editions;
using GEC.Attendance.Editions.Dto;
using GEC.Attendance.MultiTenancy.Payments;
using GEC.Attendance.MultiTenancy.Payments.Dto;

namespace GEC.Attendance.Web.Models.Payment
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
