using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.GEC.Attendance.Editions.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}