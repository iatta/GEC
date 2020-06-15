using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class CheckInInput
    {
        public int UserId { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
