using Abp.AspNetCore.Mvc.Views;

namespace Pixel.Attendance.Web.Views
{
    public abstract class AttendanceRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected AttendanceRazorPage()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}
