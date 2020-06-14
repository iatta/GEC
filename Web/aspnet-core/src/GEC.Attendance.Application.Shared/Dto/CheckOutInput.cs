using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Dto
{
    public class CheckOutInput
    {
        public int UserId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
