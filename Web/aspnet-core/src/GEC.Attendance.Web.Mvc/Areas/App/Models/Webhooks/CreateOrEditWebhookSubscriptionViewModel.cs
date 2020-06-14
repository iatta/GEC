using Abp.Application.Services.Dto;
using Abp.Webhooks;
using GEC.Attendance.WebHooks.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Webhooks
{
    public class CreateOrEditWebhookSubscriptionViewModel
    {
        public WebhookSubscription WebhookSubscription { get; set; }

        public ListResultDto<GetAllAvailableWebhooksOutput> AvailableWebhookEvents { get; set; }
    }
}
