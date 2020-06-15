using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pixel.Attendance.MultiTenancy.HostDashboard.Dto;

namespace Pixel.Attendance.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}