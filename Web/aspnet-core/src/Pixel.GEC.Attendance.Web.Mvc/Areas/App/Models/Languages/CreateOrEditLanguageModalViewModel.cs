using Abp.AutoMapper;
using Pixel.GEC.Attendance.Localization.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}