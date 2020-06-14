using System.Collections.Generic;
using Abp.Application.Services.Dto;
using GEC.Attendance.Configuration.Tenants.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}