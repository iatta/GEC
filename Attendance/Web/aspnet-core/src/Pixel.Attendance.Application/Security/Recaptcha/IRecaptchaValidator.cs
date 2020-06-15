using System.Threading.Tasks;

namespace Pixel.Attendance.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}