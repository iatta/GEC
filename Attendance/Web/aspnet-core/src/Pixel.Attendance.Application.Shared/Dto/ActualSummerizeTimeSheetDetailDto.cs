using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ActualSummerizeTimeSheetDetailDto
    {
        public ActualSummerizeTimeSheetDetailDto()
        {
            UnitsToApprove = new List<string>();
        }
        public DateTime Day { get; set; }
        public int InTransactionId { get; set; }
        public int OutTransactionId { get; set; }
        public double TotalMinutes { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsSick { get; set; }
        public bool IsWorkInAnotherProject { get; set; }
        public bool CanManagerApprove { get; set; }
        public bool CanHrApprove { get; set; }
        public bool CanProjectManagerReject { get; set; }
        public bool WaitForManagerToApprove { get; set; }
        public bool IsProjectManagerApproved { get; set; }
        public bool IsCurrentUnitApproved { get; set; }
        public bool YesClose { get; set; }
        public List<string> UnitsToApprove { get; set; }
        public bool IsLeave { get; set; }
        

        public bool IsUnpaid { get; set; }
        public bool IsDelay  { get; set; }
        public double Overtime { get; set; }
        public double Delay { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public bool IsTransferred { get; set; }
        public int LeaveMinutes { get; set; }
        public int SickMinutes { get; set; }
        public bool CanApprove { get; set; }
        public int   AbsenceMinutes { get; set; }
        public double DeductMinutes { get; set; }
        public bool IsRest { get; set; }
        public bool IsDayOff { get; set; }


    }
}
