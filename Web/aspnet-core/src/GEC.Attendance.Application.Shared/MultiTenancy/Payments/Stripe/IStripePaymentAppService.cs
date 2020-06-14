using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.MultiTenancy.Payments.Dto;
using GEC.Attendance.MultiTenancy.Payments.Stripe.Dto;

namespace GEC.Attendance.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}