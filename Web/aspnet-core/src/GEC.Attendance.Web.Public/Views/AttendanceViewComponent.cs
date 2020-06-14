using Abp.AspNetCore.Mvc.ViewComponents;

namespace GEC.Attendance.Web.Public.Views
{
    public abstract class AttendanceViewComponent : AbpViewComponent
    {
        protected AttendanceViewComponent()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}