using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.Attendance.Dto
{
    public class UnitTransactionsReportInput
    {
        public int OrganizationUnitId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
