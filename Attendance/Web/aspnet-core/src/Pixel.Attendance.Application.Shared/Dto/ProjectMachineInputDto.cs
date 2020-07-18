using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ProjectMachineInputDto
    {
        public List<ProjectMachineDto> ProjectMachines { get; set; }
        public int ProjectId { get; set; }
    }
}
