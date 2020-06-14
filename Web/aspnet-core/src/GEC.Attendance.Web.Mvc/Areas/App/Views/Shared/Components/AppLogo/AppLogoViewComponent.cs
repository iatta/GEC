using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GEC.Attendance.Web.Areas.App.Models.Layout;
using GEC.Attendance.Web.Session;
using GEC.Attendance.Web.Views;

namespace GEC.Attendance.Web.Areas.App.Views.Shared.Components.AppLogo
{
    public class AppLogoViewComponent : AttendanceViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppLogoViewComponent(
            IPerRequestSessionCache sessionCache
        )
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string logoSkin = null, string logoClass = "")
        {
            var headerModel = new LogoViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
                LogoSkinOverride = logoSkin,
                LogoClassOverride = logoClass
            };

            return View(headerModel);
        }
    }
}
