using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ActualSummerizeTimeSheetOutput
    {
        public ActualSummerizeTimeSheetOutput()
        {
            Data = new List<ActualSummerizeTimeSheetDto>();
            UserIds = new List<UserTimeSheetInput>();
            UserIdsToApprove = new List<UserTimeSheetInput>();
            ParentUnitIds = new List<long>();
            ChildtUnitIds = new List<long>();
        }
        public ICollection<ActualSummerizeTimeSheetDto> Data { get; set; }
        public List<UserTimeSheetInput> UserIds { get; set; }
        public List<long> ParentUnitIds { get; set; }
        public List<long> ChildtUnitIds { get; set; }
        public List<string> RemainingUnitsApprove { get; set; }
        
        public int TotalPending { get; set; }
        public int TotalApproved { get; set; }
        public bool CanApprove { get; set; }
        public List<UserTimeSheetInput> UserIdsToApprove { get; set; }

    }
}
