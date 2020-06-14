using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization.Users.Dto;

namespace GEC.Attendance.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
