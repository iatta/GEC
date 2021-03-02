using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ProjectManagerApproveInput
    {
        public List<UserTimeSheetInput> UserIds { get; set; }
        public int ProjectId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int UnitIdToApprove { get; set; }
        public bool IsMonth { get; set; }
        public bool IsDateRange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
