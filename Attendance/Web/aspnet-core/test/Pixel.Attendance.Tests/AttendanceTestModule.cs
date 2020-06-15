using Abp.Modules;
using Pixel.Attendance.Test.Base;

namespace Pixel.Attendance.Tests
{
    [DependsOn(typeof(AttendanceTestBaseModule))]
    public class AttendanceTestModule : AbpModule
    {
       
    }
}
