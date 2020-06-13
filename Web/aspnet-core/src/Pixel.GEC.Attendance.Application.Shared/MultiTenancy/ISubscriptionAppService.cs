using System.Threading.Tasks;
using Abp.Application.Services;

namespace Pixel.GEC.Attendance.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
