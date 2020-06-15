using System.Collections.Generic;
using Pixel.Attendance.Caching.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}