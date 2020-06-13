using Abp.AutoMapper;
using Pixel.GEC.Attendance.MultiTenancy.Dto;

namespace Pixel.GEC.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}