using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.MultiTenancy.Payments.PayPal.Dto;

namespace Pixel.Attendance.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
