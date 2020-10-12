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
            UnitsToApprove = new List<string>();
        }
        public long UserId { get; set; }
        public string Code { get; set; }
        public string FingerCode { get; set; }
        public string UserName { get; set; }
        public double TotalAttendance { get; set; }
        public int TotalDeductionHours { get; set; }
        public bool IsProjectManagerApproved { get; set; }
        public bool CanManagerApprove { get; set; }
        public bool CanProjectManagerReject { get; set; }
        public bool WaitForManagerToApprove { get; set; }
        public List<string> UnitsToApprove { get; set; }
        public bool IsCurrentUnitApproved { get; set; }
        public bool YesClose { get; set; }
      
        public ICollection<ActualSummerizeTimeSheetDetailDto> Details { get; set; }

    }
}
