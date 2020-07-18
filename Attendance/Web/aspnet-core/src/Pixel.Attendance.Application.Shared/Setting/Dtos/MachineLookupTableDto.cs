using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Setting.Dtos
{
    public class MachineLookupTableDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string IpAddress { get; set; }
        public bool IsSelected { get; set; }
    }
}
