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
    }
}
