using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.Web.Controllers;

namespace Pixel.Attendance.Web.Public.Controllers
{
    public class AboutController : AttendanceControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}