using System.Collections.Generic;
using Pixel.GEC.Attendance.Authorization.Permissions.Dto;

namespace Pixel.GEC.Attendance.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}