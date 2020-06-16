using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public  class ProjectUserInputDto
    {
        public List<ProjectUserDto> ProjectUsers { get; set; }
        public int ProjectId { get; set; }

    }
}
