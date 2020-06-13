﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pixel.GEC.Attendance.ReportsModel
{
    public class PermitReportCore
    {
        [Key]
        public int PermitId { get; set; }
        public DateTime Date { get; set; }
        public string TypeEn { get; set; }
        public string TypeAr { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }

        public string KindAr { get; set; }
        public string KindEn { get; set; }

        public int ToTime { get; set; }
        public int FromTime { get; set; }
        public long? EmpId { get; set; }

        
        public string UserName { get; set; }




    }
}
