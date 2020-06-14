using GEC.Attendance.Attendance;
using GEC.Attendance.Operations;
using GEC.Attendance.Setting;
using System;
using System.Linq;
using Abp.Organizations;
using GEC.Attendance.Authorization.Roles;
using GEC.Attendance.MultiTenancy;

namespace GEC.Attendance.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(Project),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(Project),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
