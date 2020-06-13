using Microsoft.AspNetCore.Antiforgery;

namespace Pixel.GEC.Attendance.Web.Controllers
{
    public class AntiForgeryController : AttendanceControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
