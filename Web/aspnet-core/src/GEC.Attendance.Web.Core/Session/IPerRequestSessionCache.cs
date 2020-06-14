using System.Threading.Tasks;
using GEC.Attendance.Sessions.Dto;

namespace GEC.Attendance.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
