using Abp.AspNetCore.Mvc.ViewComponents;

namespace Pixel.GEC.Attendance.Web.Public.Views
{
    public abstract class AttendanceViewComponent : AbpViewComponent
    {
        protected AttendanceViewComponent()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}