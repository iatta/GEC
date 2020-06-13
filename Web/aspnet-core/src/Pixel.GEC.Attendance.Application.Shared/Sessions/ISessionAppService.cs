using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.GEC.Attendance.Sessions.Dto;

namespace Pixel.GEC.Attendance.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
