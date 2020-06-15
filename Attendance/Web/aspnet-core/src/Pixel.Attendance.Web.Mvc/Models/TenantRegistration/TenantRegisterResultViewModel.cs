using Abp.AutoMapper;
using Pixel.Attendance.MultiTenancy.Dto;

namespace Pixel.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}