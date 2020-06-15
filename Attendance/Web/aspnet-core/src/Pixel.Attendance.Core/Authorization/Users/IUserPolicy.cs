using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace Pixel.Attendance.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
