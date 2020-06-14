using System.Collections.Generic;
using GEC.Attendance.Editions.Dto;
using GEC.Attendance.MultiTenancy.Payments;

namespace GEC.Attendance.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}