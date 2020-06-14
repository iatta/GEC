using Abp.Modules;
using GEC.Attendance.Test.Base;

namespace GEC.Attendance.Tests
{
    [DependsOn(typeof(AttendanceTestBaseModule))]
    public class AttendanceTestModule : AbpModule
    {
       
    }
}
