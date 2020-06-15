using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Setting
{
   public class PermitType
    {
        public int PermitId { get; set; }
        public int TypesOfPermitId { get; set; }

        public Permit Permit { get; set; }
        public TypesOfPermit TypesOfPermit { get; set; }

    }
}
