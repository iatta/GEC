using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Layout;
using Pixel.GEC.Attendance.Web.Session;
using Pixel.GEC.Attendance.Web.Views;

namespace Pixel.GEC.Attendance.Web.Areas.App.Views.Shared.Components.AppTheme5Footer
{
    public class AppTheme5FooterViewComponent : AttendanceViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme5FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
