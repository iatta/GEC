using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Pixel.Attendance.Editions.Dto;
using Pixel.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.Attendance.Web.Areas.App.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class CreateEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}