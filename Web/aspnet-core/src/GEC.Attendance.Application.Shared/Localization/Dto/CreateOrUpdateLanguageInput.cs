using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}