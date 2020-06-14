using Abp.Auditing;
using GEC.Attendance.Configuration.Dto;

namespace GEC.Attendance.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}