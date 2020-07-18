using Pixel.Attendance.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pixel.Attendance.Operations
{
    public class ProjectMachine
    {
        public int MachineId { get; set; }
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        [ForeignKey("MachineId")]
        public Machine Machine { get; set; }
    }
}
