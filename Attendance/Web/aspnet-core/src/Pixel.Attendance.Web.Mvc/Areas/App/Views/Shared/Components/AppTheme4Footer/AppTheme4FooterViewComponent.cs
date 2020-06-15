using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.Web.Areas.App.Models.Layout;
using Pixel.Attendance.Web.Session;
using Pixel.Attendance.Web.Views;

namespace Pixel.Attendance.Web.Areas.App.Views.Shared.Components.AppTheme4Footer
{
    public class AppTheme4FooterViewComponent : AttendanceViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme4FooterViewComponent(IPerRequestSessionCache sessionCache)
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
