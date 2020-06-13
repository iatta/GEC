using Abp.Authorization;
using Pixel.GEC.Attendance.Authorization.Roles;
using Pixel.GEC.Attendance.Authorization.Users;

namespace Pixel.GEC.Attendance.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
