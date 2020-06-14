using System.Collections.Generic;
using GEC.Attendance.Authorization.Users.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}