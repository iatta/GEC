using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Pixel.GEC.Attendance.Authorization.Users;
using Pixel.GEC.Attendance.MultiTenancy;

namespace Pixel.GEC.Attendance.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}