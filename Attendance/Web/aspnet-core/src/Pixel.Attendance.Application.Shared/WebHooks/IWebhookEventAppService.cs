using System.Threading.Tasks;
using Abp.Webhooks;

namespace Pixel.Attendance.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
