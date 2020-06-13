using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.GEC.Attendance.MultiTenancy.Payments.PayPal.Dto;

namespace Pixel.GEC.Attendance.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
