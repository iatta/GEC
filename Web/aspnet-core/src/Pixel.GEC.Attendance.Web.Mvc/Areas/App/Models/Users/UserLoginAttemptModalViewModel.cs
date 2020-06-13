using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Users.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}