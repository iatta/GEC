using Abp.Authorization;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Authorization.Users;

namespace Pixel.Attendance.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
