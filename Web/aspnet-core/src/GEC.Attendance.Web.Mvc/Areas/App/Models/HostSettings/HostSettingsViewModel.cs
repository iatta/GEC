using System.Collections.Generic;
using Abp.Application.Services.Dto;
using GEC.Attendance.Configuration.Host.Dto;
using GEC.Attendance.Editions.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}