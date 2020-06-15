using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Pixel.Attendance.Editions.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}