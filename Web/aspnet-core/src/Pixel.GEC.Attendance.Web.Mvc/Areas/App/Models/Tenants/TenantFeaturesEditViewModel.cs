using Abp.AutoMapper;
using Pixel.GEC.Attendance.MultiTenancy;
using Pixel.GEC.Attendance.MultiTenancy.Dto;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}