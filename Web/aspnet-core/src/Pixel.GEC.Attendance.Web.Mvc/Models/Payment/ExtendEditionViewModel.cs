using System.Collections.Generic;
using Pixel.GEC.Attendance.Editions.Dto;
using Pixel.GEC.Attendance.MultiTenancy.Payments;

namespace Pixel.GEC.Attendance.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}