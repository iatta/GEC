﻿using System.Collections.Generic;
using Pixel.GEC.Attendance.Editions.Dto;
using Pixel.GEC.Attendance.MultiTenancy.Payments;

namespace Pixel.GEC.Attendance.Web.Models.Payment
{
    public class UpgradeEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}