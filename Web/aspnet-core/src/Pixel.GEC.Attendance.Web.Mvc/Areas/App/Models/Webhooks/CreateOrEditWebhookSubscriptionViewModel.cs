using Abp.Application.Services.Dto;
using Abp.Webhooks;
using Pixel.GEC.Attendance.WebHooks.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
