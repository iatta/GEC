using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class DownloadImageInput
    {
        public int UserCode { get; set; }
        public string Image { get; set; }
        public byte[] Datas { get; set; }
        public MachineData MachineData { get; set; }
    }
}
