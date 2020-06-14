using Abp.Modules;
using Abp.Reflection.Extensions;

namespace GEC.Attendance
{
    [DependsOn(typeof(AttendanceCoreSharedModule))]
    public class AttendanceApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceApplicationSharedModule).GetAssembly());
        }
    }
}