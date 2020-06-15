using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Operations.Dtos
{
    public class CreateTimeProfileFromExcelDto
    {
        public string EmployeeCode { get; set; }
        public DateTime Date { get; set; }
        public string ShiftType { get; set; }
        public string Shift { get; set; }
    }
}
