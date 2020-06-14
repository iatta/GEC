using System.Collections.Generic;
using Abp.Localization;
using GEC.Attendance.Install.Dto;

namespace GEC.Attendance.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
