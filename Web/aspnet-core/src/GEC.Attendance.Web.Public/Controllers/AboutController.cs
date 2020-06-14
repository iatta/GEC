using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Web.Controllers;

namespace GEC.Attendance.Web.Public.Controllers
{
    public class AboutController : AttendanceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}