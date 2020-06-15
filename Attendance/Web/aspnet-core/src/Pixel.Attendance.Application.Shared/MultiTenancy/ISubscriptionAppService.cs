using System.Threading.Tasks;
using Abp.Application.Services;

namespace Pixel.Attendance.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
