using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class UserLocationDto
    {
        public long UserId { get; set; }
        public int LocationId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string LocationDisplayName { get; set; }

        public bool IsAssigned { get; set; }
    }
}
