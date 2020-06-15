using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.Sessions.Dto;

namespace Pixel.Attendance.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
