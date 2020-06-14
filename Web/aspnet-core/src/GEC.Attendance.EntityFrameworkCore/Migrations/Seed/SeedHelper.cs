using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using GEC.Attendance.EntityFrameworkCore;
using GEC.Attendance.Migrations.Seed.Host;
using GEC.Attendance.Migrations.Seed.Tenants;
using GEC.Attendance.Setting;

namespace GEC.Attendance.Migrations.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<AttendanceDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(AttendanceDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            //Host seed
            new InitialHostDbBuilder(context).Create();

            //Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();

            if (!context.SystemConfigurations.Any())
            {
                var systemConfigurationToAdd = new SystemConfiguration();
                systemConfigurationToAdd.TotalPermissionNumberPerDay = 0;
                systemConfigurationToAdd.TotalPermissionNumberPerMonth = 0;
                systemConfigurationToAdd.TotalPermissionNumberPerWeek = 0;
                systemConfigurationToAdd.TotalPermissionHoursPerWeek = 0;
                systemConfigurationToAdd.TotalPermissionHoursPerDay = 0;
                systemConfigurationToAdd.TotalPermissionHoursPerMonth = 0;

                context.SystemConfigurations.Add(systemConfigurationToAdd);
                context.SaveChanges();
            }
        }

       
        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
