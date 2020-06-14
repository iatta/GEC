using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Web.Areas.App.Models.Layout;
using GEC.Attendance.Web.Session;
using GEC.Attendance.Web.Views;

namespace GEC.Attendance.Web.Areas.App.Views.Shared.Components.AppTheme4Footer
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
