using System.Threading.Tasks;

namespace GEC.Attendance.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}