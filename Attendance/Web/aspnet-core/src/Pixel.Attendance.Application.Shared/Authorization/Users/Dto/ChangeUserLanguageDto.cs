using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
