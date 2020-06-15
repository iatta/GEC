using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class CheckInOutput
    {
        public string UserName { get; set; }
        public string Position { get; set; }

        public DateTime CheckinTime { get; set; }
        public bool Status { get; set; }

    }
}
