using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pixel.GEC.Attendance.MultiTenancy.HostDashboard.Dto;

namespace Pixel.GEC.Attendance.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}