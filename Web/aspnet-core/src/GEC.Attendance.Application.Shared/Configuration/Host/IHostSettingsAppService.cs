using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.Configuration.Host.Dto;

namespace GEC.Attendance.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
