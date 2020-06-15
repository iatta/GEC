using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.MultiTenancy.Payments.Dto;
using Pixel.Attendance.MultiTenancy.Payments.Stripe.Dto;

namespace Pixel.Attendance.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}