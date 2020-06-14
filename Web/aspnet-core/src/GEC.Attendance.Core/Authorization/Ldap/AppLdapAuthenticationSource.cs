using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using GEC.Attendance.Authorization.Users;
using GEC.Attendance.MultiTenancy;

namespace GEC.Attendance.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}