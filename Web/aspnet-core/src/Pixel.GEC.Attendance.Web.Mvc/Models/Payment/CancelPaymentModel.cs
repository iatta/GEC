﻿using Pixel.GEC.Attendance.MultiTenancy.Payments;

namespace Pixel.GEC.Attendance.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}