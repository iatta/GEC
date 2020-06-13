using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Pixel.GEC.Attendance
{
    public class AttendanceClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AttendanceClientModule).GetAssembly());
        }
    }
}
