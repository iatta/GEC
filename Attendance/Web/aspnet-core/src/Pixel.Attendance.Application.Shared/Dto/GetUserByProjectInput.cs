﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class GetUserByProjectInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ProjectId { get; set; }
        public int? UserType { get; set; }
    }
}
