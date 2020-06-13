using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Pixel.GEC.Attendance.Startup
{
    [DependsOn(typeof(AttendanceCoreModule))]
    public class AttendanceGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}