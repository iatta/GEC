using Abp.AutoMapper;
using GEC.Attendance.MultiTenancy.Dto;

namespace GEC.Attendance.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}