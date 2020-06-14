using Abp.Domain.Services;

namespace GEC.Attendance
{
    public abstract class AttendanceDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AttendanceDomainServiceBase()
        {
            LocalizationSourceName = AttendanceConsts.LocalizationSourceName;
        }
    }
}
