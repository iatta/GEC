using Abp.AutoMapper;
using Pixel.Attendance.MultiTenancy;
using Pixel.Attendance.MultiTenancy.Dto;
using Pixel.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.Attendance.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}