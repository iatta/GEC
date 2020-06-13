using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Layout;
using Pixel.GEC.Attendance.Web.Session;
using Pixel.GEC.Attendance.Web.Views;

namespace Pixel.GEC.Attendance.Web.Areas.App.Views.Shared.Components.AppTheme4Brand
{
    public class AppTheme4BrandViewComponent : AttendanceViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme4BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
