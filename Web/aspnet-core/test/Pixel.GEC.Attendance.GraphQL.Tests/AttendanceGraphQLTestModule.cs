using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Pixel.GEC.Attendance.Configure;
using Pixel.GEC.Attendance.Startup;
using Pixel.GEC.Attendance.Test.Base;

namespace Pixel.GEC.Attendance.GraphQL.Tests
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