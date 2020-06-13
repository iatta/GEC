﻿using Pixel.GEC.Attendance.Editions;

namespace Pixel.GEC.Attendance.MultiTenancy.Payments.Dto
{
    public class CreatePaymentDto
    {
        public int EditionId { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public SubscriptionPaymentGatewayType SubscriptionPaymentGatewayType { get; set; }

        public bool RecurringPaymentEnabled { get; set; }

        public string SuccessUrl { get; set; }

        public string ErrorUrl { get; set; }
    }
}
