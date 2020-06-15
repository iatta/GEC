using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Configuration.Host.Dto;
using Pixel.Attendance.Editions.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.HostSettings
{
    public class HostSettingsViewModel
    {
        public HostSettingsEditDto Settings { get; set; }

        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public List<ComboboxItemDto> TimezoneItems { get; set; }
    }
}