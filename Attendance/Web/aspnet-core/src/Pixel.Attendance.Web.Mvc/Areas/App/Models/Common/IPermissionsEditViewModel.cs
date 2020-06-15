using System.Collections.Generic;
using Pixel.Attendance.Authorization.Permissions.Dto;

namespace Pixel.Attendance.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}