using System.Collections.Generic;
using Abp.Application.Services.Dto;
using GEC.Attendance.Authorization.Permissions.Dto;
using GEC.Attendance.Web.Areas.App.Models.Common;

namespace GEC.Attendance.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}