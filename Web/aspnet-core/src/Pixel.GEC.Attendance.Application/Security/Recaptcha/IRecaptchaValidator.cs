using System.Threading.Tasks;

namespace Pixel.GEC.Attendance.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}