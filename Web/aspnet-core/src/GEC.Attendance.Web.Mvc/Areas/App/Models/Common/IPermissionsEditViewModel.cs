using System.Collections.Generic;
using GEC.Attendance.Authorization.Permissions.Dto;

namespace GEC.Attendance.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}