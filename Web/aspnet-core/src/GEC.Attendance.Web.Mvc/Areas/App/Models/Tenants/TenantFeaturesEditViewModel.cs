using Abp.AutoMapper;
using GEC.Attendance.MultiTenancy;
using GEC.Attendance.MultiTenancy.Dto;
using GEC.Attendance.Web.Areas.App.Models.Common;

namespace GEC.Attendance.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}