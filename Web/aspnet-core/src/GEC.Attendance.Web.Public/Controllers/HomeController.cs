using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Web.Controllers;

namespace GEC.Attendance.Web.Public.Controllers
{
    public class HomeController : AttendanceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}