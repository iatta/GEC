using System.Threading.Tasks;
using Abp.Application.Services;

namespace GEC.Attendance.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
