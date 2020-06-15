using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Editions.Dto;

namespace Pixel.Attendance.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}