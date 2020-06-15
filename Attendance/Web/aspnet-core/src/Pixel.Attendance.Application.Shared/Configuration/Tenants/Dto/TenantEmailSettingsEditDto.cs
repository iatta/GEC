using Abp.Auditing;
using Pixel.Attendance.Configuration.Dto;

namespace Pixel.Attendance.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}