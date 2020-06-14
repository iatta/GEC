using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace GEC.Attendance.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
