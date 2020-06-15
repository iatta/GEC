using Abp.AutoMapper;
using Pixel.Attendance.Authorization.Roles.Dto;
using Pixel.Attendance.Web.Areas.App.Models.Common;

namespace Pixel.Attendance.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}