using System.Threading.Tasks;
using Pixel.Attendance.Authorization.Users;

namespace Pixel.Attendance.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
