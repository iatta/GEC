using System.ComponentModel.DataAnnotations;

namespace Pixel.Attendance.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}