using Abp.AutoMapper;
using GEC.Attendance.Authorization.Roles.Dto;
using GEC.Attendance.Web.Areas.App.Models.Common;

namespace GEC.Attendance.Web.Areas.App.Models.Roles
{
    [AutoMapFrom(typeof(GetRoleForEditOutput))]
    public class CreateOrEditRoleModalViewModel : GetRoleForEditOutput, IPermissionsEditViewModel
    {
        public bool IsEditMode => Role.Id.HasValue;
    }
}