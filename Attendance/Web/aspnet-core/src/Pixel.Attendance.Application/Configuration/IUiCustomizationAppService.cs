using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.Configuration.Dto;

namespace Pixel.Attendance.Configuration
{
    public interface IUiCustomizationSettingsAppService : IApplicationService
    {
        Task<List<ThemeSettingsDto>> GetUiManagementSettings();

        Task UpdateUiManagementSettings(ThemeSettingsDto settings);

        Task UpdateDefaultUiManagementSettings(ThemeSettingsDto settings);

        Task UseSystemDefaultSettings();
    }
}
