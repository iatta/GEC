using System.Threading.Tasks;
using Abp.Net.Mail;
using GEC.Attendance.Configuration.Host.Dto;

namespace GEC.Attendance.Configuration
{
    public abstract class SettingsAppServiceBase : AttendanceAppServiceBase
    {
        private readonly IEmailSender _emailSender;

        protected SettingsAppServiceBase(
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #region Send Test Email

        public async Task SendTestEmail(SendTestEmailInput input)
        {
            await _emailSender.SendAsync(
                input.EmailAddress,
                L("TestEmail_Subject"),
                L("TestEmail_Body")
            );
        }

        #endregion
    }
}
