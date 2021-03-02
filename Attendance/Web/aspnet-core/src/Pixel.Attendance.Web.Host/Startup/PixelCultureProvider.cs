using Abp.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pixel.Attendance.Web.Startup
{
    public class PixelCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var requestCultureProvider = new AbpDefaultRequestCultureProvider();
            var result = await requestCultureProvider.DetermineProviderCultureResult(httpContext);
            if (!string.IsNullOrEmpty(httpContext.Request.Query["cultureName"]))
            {
                return new ProviderCultureResult(culture: "en-Us", uiCulture: httpContext.Request.Query["cultureName"].ToString());
            }
            if (result.Cultures[0].Buffer == "ar")
            {
                return new ProviderCultureResult(culture: "en-US", uiCulture: "ar");
            }
            return result;
        }
    }
}
