using System.Threading.Tasks;
using Pixel.GEC.Attendance.Authorization.Users;

namespace Pixel.GEC.Attendance.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
