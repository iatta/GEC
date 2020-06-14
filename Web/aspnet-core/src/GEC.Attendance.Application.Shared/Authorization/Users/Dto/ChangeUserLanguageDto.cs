using System.ComponentModel.DataAnnotations;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
