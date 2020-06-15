using System.Threading.Tasks;
using Pixel.Attendance.Security.Recaptcha;

namespace Pixel.Attendance.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
