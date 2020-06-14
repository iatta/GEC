using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class UserReportDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ShiftName { get; set; }
        public int ShiftId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
