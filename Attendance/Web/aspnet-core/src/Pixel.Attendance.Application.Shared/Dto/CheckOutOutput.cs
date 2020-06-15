using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class CheckOutOutput
    {
        public string UserName { get; set; }
        public string Position { get; set; }

        public DateTime CheckoutTime { get; set; }
        public bool Status { get; set; }
    }
}
