using System.Threading.Tasks;

namespace Pixel.Attendance.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}