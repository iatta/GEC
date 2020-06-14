using System.Collections.Generic;
using Abp.Application.Services.Dto;
using GEC.Attendance.Editions.Dto;

namespace GEC.Attendance.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}