using System.Collections.Generic;
using GEC.Attendance.Authorization.Permissions.Dto;

namespace GEC.Attendance.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}