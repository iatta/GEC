using Abp.AutoMapper;
using Pixel.GEC.Attendance.Sessions.Dto;

namespace Pixel.GEC.Attendance.Web.Views.Shared.Components.TenantChange
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}