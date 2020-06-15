using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Pixel.Attendance.Configure;
using Pixel.Attendance.Startup;
using Pixel.Attendance.Test.Base;

namespace Pixel.Attendance.GraphQL.Tests
{
    [DependsOn(
        typeof(AttendanceGraphQLModule),
        typeof(AttendanceTestBaseModule))]
    public class AttendanceGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceGraphQLTestModule).GetAssembly());
        }
    }
}