using System.Threading.Tasks;
using Abp.Webhooks;

namespace Pixel.GEC.Attendance.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
