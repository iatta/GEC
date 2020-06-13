﻿using Microsoft.Extensions.DependencyInjection;
using Pixel.GEC.Attendance.HealthChecks;

namespace Pixel.GEC.Attendance.Web.HealthCheck
{
    public static class AbpZeroHealthCheck
    {
        public static IHealthChecksBuilder AddAbpZeroHealthCheck(this IServiceCollection services)
        {
            var builder = services.AddHealthChecks();
            builder.AddCheck<AttendanceDbContextHealthCheck>("Database Connection");
            builder.AddCheck<AttendanceDbContextUsersHealthCheck>("Database Connection with user check");
            builder.AddCheck<CacheHealthCheck>("Cache");

            // add your custom health checks here
            // builder.AddCheck<MyCustomHealthCheck>("my health check");

            return builder;
        }
    }
}
