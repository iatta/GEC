using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class AssignedTypesOfPermitDto
    {
        public int TypeOfPermitId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool Assigned { get; set; }
    }
}
