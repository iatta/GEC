﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Editions.Dto;

namespace Pixel.GEC.Attendance.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}