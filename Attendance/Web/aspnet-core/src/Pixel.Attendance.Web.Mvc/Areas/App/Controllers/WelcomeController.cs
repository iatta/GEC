using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.Web.Controllers;

namespace Pixel.Attendance.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class WelcomeController : AttendanceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}