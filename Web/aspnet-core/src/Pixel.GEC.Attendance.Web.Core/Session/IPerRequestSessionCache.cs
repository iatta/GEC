using System.Threading.Tasks;
using Pixel.GEC.Attendance.Sessions.Dto;

namespace Pixel.GEC.Attendance.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
