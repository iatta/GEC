using System.ComponentModel.DataAnnotations;

namespace Pixel.GEC.Attendance.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}