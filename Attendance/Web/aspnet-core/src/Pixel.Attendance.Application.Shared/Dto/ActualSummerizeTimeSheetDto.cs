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
        public string UserName { get; set; }
        public double TotalAttendance { get; set; }
        public int TotalDeductionHours { get; set; }

        public ICollection<ActualSummerizeTimeSheetDetailDto> Details { get; set; }

    }
}
