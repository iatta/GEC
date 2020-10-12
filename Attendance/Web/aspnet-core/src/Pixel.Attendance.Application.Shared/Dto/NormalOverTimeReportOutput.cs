using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class NormalOverTimeReportOutput
    {
        public string BusinessUnit { get; set; }
        public string DocumentEntry { get; set; }
        public DateTime AttendanceDate { get; set; }
        public  string PersonName { get; set; }
        public string PersonNumber { get; set; }
        public string ProjectName { get; set; }
        public string ProjectNumber { get; set; }
        public string TaskName { get; set; }
        public string TaskNo { get; set; }
        public string ExpenditureType { get; set; }
        public double Hours { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
    }
}
