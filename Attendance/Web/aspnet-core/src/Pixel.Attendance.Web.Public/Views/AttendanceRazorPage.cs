using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Pixel.Attendance.Web.Public.Views
{
    public abstract class AttendanceRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected AttendanceRazorPage()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}
