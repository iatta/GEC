using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.GEC.Attendance.Authorization.Users.Dto
{
    public class EmployeeReportOutput
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int AbsentCount { get; set; }
        public int AttendanceCount { get; set; }

        public int AbsentContinusDays { get; set; }

        public int DaysCount { get; set; }
    }
}
