using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Configuration.Host.Dto;
using Pixel.GEC.Attendance.Editions.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}