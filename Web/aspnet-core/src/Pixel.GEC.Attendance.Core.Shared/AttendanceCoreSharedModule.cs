using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Pixel.GEC.Attendance
{
    public class AttendanceCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceCoreSharedModule).GetAssembly());
        }
    }
}