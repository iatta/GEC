using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace GEC.Attendance
{
    [DependsOn(typeof(AttendanceClientModule), typeof(AbpAutoMapperModule))]
    public class AttendanceXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceXamarinSharedModule).GetAssembly());
        }
    }
}