using Abp.AutoMapper;
using GEC.Attendance.MultiTenancy.Dto;

namespace GEC.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
