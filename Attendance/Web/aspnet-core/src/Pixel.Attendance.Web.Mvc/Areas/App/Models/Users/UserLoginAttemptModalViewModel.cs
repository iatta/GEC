using System.Collections.Generic;
using Pixel.Attendance.Authorization.Users.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}