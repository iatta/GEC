using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class ReadAllUsersOutput
    {
        public List<Person> PersonList { get; set; }
        public int DataBaseSize { get; set; }
    }
}
