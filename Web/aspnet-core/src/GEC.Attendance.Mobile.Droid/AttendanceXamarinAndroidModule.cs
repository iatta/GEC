using Abp.Modules;
using Abp.Reflection.Extensions;

namespace GEC.Attendance
{
    [DependsOn(typeof(AttendanceXamarinSharedModule))]
    public class AttendanceXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceXamarinAndroidModule).GetAssembly());
        }
    }
}