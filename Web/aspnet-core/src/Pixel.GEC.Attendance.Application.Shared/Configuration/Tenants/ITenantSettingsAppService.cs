using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.GEC.Attendance.Configuration.Tenants.Dto;

namespace Pixel.GEC.Attendance.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
