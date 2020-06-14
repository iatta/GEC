using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class ReportInput
    {
        public int ReportId { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int[] UserIds { get; set; }

        public int Location { get; set; }

        public int Machine { get; set; }

        public int DaysCount { get; set; }
        public int Type { get; set; }
    }
}
