using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Pixel.Attendance.Authorization;

namespace Pixel.Attendance
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AttendanceApplicationSharedModule),
        typeof(AttendanceCoreModule)
        )]
    public class AttendanceApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceApplicationModule).GetAssembly());
        }
    }
}