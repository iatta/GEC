using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.Sessions.Dto;

namespace GEC.Attendance.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
