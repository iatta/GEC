using System.Threading.Tasks;
using Abp.Authorization.Users;
using Pixel.GEC.Attendance.Authorization.Users;

namespace Pixel.GEC.Attendance.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
