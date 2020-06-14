using System.Collections.Generic;
using GEC.Attendance.DashboardCustomization.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
