using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.Authorization;
using Pixel.GEC.Attendance.DashboardCustomization;
using Pixel.GEC.Attendance.Web.DashboardCustomization;
using System.Threading.Tasks;

namespace Pixel.GEC.Attendance.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Host_Dashboard)]
    public class HostDashboardController : CustomizableDashboardControllerBase
    {
        public HostDashboardController(
            DashboardViewConfiguration dashboardViewConfiguration,
            IDashboardCustomizationAppService dashboardCustomizationAppService)
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(AttendanceDashboardCustomizationConsts.DashboardNames.DefaultHostDashboard);
        }
    }
}