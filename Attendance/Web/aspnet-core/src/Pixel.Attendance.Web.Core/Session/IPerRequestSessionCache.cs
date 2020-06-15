using System.Threading.Tasks;
using Pixel.Attendance.Sessions.Dto;

namespace Pixel.Attendance.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
