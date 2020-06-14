using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace GEC.Attendance.Web.Controllers
{
    public class HomeController : AttendanceControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
