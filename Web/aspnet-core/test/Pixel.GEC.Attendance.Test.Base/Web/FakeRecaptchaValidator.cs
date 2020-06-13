using System.Threading.Tasks;
using Pixel.GEC.Attendance.Security.Recaptcha;

namespace Pixel.GEC.Attendance.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
