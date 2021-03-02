using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class UploadMachineUserInput
    {
        public Person Person { get; set; }
        public MachineData MachineData { get; set; }
    }
}
