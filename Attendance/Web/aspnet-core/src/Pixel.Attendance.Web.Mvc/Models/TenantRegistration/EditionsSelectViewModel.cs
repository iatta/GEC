using Abp.AutoMapper;
using Pixel.Attendance.MultiTenancy.Dto;

namespace Pixel.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
