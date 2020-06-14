using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GEC.Attendance.ReportsModel
{
    public class EmployeeReportCore
    {
        [Key]
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int AbsentCount { get; set; }
        public int AttendanceCount { get; set; }

        public int AbsentContinusDays { get; set; }

        public int DaysCount { get; set; }
    }
}
