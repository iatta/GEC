using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ActualSummerizeTimeSheetDetailDto
    {
        public DateTime Day { get; set; }
        public double TotalHours { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsSick { get; set; }
        public bool IsWorkInAnotherProject { get; set; }
        public bool IsLeave { get; set; }

        public bool IsUnpaid { get; set; }
        public bool IsDelay  { get; set; }
        public double Overtime { get; set; }
        public double Delay { get; set; }

    }
}
