using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ActualSummerizeInput
    {
        public ActualSummerizeInput()
        {
            UserIds = new long[] { };
        }
        public int ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int? UserType { get; set; }
        public long[] UserIds { get; set; }
        public bool IsMonth { get; set; }
        public bool IsDateRange { get; set; }

    }
}
