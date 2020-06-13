using Abp.AutoMapper;
using Pixel.GEC.Attendance.Authorization.Roles.Dto;
using Pixel.GEC.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}