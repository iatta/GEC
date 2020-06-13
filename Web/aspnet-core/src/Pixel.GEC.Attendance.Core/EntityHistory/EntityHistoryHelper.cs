using Pixel.GEC.Attendance.Attendance;
using Pixel.GEC.Attendance.Operations;
using Pixel.GEC.Attendance.Setting;
using System;
using System.Linq;
using Abp.Organizations;
using Pixel.GEC.Attendance.Authorization.Roles;
using Pixel.GEC.Attendance.MultiTenancy;

namespace Pixel.GEC.Attendance.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
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
