using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class UserTimeSheetInput
    {
        public UserTimeSheetInput()
        {
            DaysToApprove = new List<DateTime>();
        }
        public long UserId { get; set; }
        public bool YesClose { get; set; }
        public List<DateTime> DaysToApprove { get; set; }
    }
}
