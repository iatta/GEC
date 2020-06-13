using System.Collections.Generic;
using Pixel.GEC.Attendance.Caching.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}