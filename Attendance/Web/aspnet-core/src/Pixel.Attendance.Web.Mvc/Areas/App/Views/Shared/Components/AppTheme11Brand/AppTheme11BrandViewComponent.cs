﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pixel.Attendance.Web.Areas.App.Models.Layout;
using Pixel.Attendance.Web.Session;
using Pixel.Attendance.Web.Views;

namespace Pixel.Attendance.Web.Areas.App.Views.Shared.Components.AppTheme11Brand
{
    public class AppTheme11BrandViewComponent : AttendanceViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme11BrandViewComponent(IPerRequestSessionCache sessionCache)
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
