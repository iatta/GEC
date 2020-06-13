using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Permissions.Dto;

namespace Pixel.GEC.Attendance.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}