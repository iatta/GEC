using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pixel.Attendance.ReportsModel
{
    public class ForgetInOutCore
    {
        [Key]
        public int Id { get; set; }

        public DateTime? AttendanceDate { get; set; }
        public string AttendanceInStr { get; set; }
        public string AttendanceOutStr { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }

    }
}
