using System;
using System.Collections.Generic;
using System.Text;

namespace GEC.Attendance.Dto
{
    public class AssignedLocationDto
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate{ get; set; }
        public string LocationDisplayName { get; set; }
    }
}
