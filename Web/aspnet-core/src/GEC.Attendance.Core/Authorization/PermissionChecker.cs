using Abp.Authorization;
using GEC.Attendance.Authorization.Roles;
using GEC.Attendance.Authorization.Users;

namespace GEC.Attendance.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
