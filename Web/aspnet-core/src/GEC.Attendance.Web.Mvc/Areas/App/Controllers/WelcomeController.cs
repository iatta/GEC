using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Web.Controllers;

namespace GEC.Attendance.Web.Areas.App.Controllers
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