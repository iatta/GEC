using System.Collections.Generic;

namespace Pixel.GEC.Attendance.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
