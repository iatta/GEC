using Abp.AutoMapper;
using Pixel.GEC.Attendance.MultiTenancy.Dto;

namespace Pixel.GEC.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
