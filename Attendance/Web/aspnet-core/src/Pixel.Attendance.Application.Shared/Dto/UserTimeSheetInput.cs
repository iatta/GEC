using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class UserTimeSheetInput
    {
        public long UserId { get; set; }
        public bool YesClose { get; set; }
    }
}
