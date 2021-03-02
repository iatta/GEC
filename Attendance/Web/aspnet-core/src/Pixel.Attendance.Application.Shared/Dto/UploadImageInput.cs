using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
   public class UploadImageInput
    {
        public int UserCode { get; set; }
        public MachineData MachineData { get; set; }
    }
}
