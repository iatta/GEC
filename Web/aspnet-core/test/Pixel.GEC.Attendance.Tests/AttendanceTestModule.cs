using Abp.Modules;
using Pixel.GEC.Attendance.Test.Base;

namespace Pixel.GEC.Attendance.Tests
{
    [DependsOn(typeof(AttendanceTestBaseModule))]
    public class AttendanceTestModule : AbpModule
    {
       
    }
}
