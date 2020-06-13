using Abp.Auditing;
using Pixel.GEC.Attendance.Configuration.Dto;

namespace Pixel.GEC.Attendance.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}