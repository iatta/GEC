using System.ComponentModel.DataAnnotations;
using Abp.Localization;

namespace Pixel.GEC.Attendance.Localization.Dto
{
    public class SetDefaultLanguageInput
    {
        [Required]
        [StringLength(ApplicationLanguage.MaxNameLength)]
        public virtual string Name { get; set; }
    }
}