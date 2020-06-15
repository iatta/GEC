using Pixel.Attendance.Attendance;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Setting;
using System;
using System.Linq;
using Abp.Organizations;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.MultiTenancy;

namespace Pixel.Attendance.EntityHistory
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
