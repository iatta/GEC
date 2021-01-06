using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ActualSummerizeTimeSheetDto
    {
        public ActualSummerizeTimeSheetDto()
        {
            Details = new List<ActualSummerizeTimeSheetDetailDto>();
            
        }
        public long UserId { get; set; }
        public string Code { get; set; }
        public string FingerCode { get; set; }
        public string UserName { get; set; }
        public double TotalAttendance { get; set; }
        public double TotalDeductionMinutes { get; set; }
        public double TotalSickLeaveMinutes { get; set; }
        public double TotalaAbsenceMinutes { get; set; }
        public double TotalLeaveMinutes { get; set; }
        public double TotalOverTimeNormal { get; set; }
        public double TotalOverTimeFriday { get; set; }
        public double TotalOverTimeHolidays { get; set; }
        public double TotalFOT { get; set; }

        public ICollection<ActualSummerizeTimeSheetDetailDto> Details { get; set; }

    }
}
