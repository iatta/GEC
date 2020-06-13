using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace Pixel.GEC.Attendance.Localization
{
    public static class AttendanceLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AttendanceConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(AttendanceLocalizationConfigurer).GetAssembly(),
                        "Pixel.GEC.Attendance.Localization.Attendance"
                    )
                )
            );
        }
    }
}