using Abp.Application.Services.Dto;
using Abp.Webhooks;
using Pixel.Attendance.WebHooks.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
