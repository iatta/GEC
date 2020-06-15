﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Pixel.Attendance.Configuration.Host.Dto;

namespace Pixel.Attendance.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
