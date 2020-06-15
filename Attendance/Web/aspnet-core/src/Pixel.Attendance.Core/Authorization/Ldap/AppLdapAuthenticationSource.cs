using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.MultiTenancy;

namespace Pixel.Attendance.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}