using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.Authorization;
using Pixel.Attendance.DashboardCustomization;
using Pixel.Attendance.Web.DashboardCustomization;
using System.Threading.Tasks;

namespace Pixel.Attendance.Web.Areas.App.Controllers
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