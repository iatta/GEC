using System.Collections.Generic;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.MultiTenancy.Payments;

namespace Pixel.Attendance.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}