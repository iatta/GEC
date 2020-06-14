using System.Threading.Tasks;
using Abp.Application.Services;
using GEC.Attendance.Configuration.Tenants.Dto;

namespace GEC.Attendance.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
