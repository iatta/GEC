using Abp.AspNetCore.Mvc.ViewComponents;

namespace Pixel.Attendance.Web.Views
{
    public abstract class AttendanceViewComponent : AbpViewComponent
    {
        protected AttendanceViewComponent()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}