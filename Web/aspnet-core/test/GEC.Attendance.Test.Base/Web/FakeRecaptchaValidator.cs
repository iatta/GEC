using System.Threading.Tasks;
using GEC.Attendance.Security.Recaptcha;

namespace GEC.Attendance.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
