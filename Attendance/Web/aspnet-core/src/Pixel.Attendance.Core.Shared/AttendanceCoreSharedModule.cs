using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Pixel.Attendance
{
    public class AttendanceCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceCoreSharedModule).GetAssembly());
        }
    }
}