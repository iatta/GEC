using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.Web.Controllers;

namespace Pixel.GEC.Attendance.Web.Public.Controllers
{
    public class AboutController : AttendanceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}