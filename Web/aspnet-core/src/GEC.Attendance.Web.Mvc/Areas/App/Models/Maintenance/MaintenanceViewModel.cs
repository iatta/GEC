using System.Collections.Generic;
using GEC.Attendance.Caching.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}