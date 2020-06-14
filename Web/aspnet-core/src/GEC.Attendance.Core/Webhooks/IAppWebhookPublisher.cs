using System.Threading.Tasks;
using GEC.Attendance.Authorization.Users;

namespace GEC.Attendance.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
