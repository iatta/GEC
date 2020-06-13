using System;
using System.Collections.Generic;
using System.Text;

namespace Pixel.GEC.Attendance.Tenants.Dashboard.Dto
{
    public class GetTopStatsOutput
    {
        public int TotalProfit { get; set; }

        public int NewFeedbacks { get; set; }

        public int NewOrders { get; set; }

        public int NewUsers { get; set; }
        public int TotalMachinesCount { get; set; }
        public int TotalEmployeesCount { get; set; }

        public int TotalEmployeePermits { get; set; }
        public int TotalEmployeeOfficialTasks { get; set; }
    }
}
