using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Permissions.Dto;

namespace Pixel.GEC.Attendance.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}