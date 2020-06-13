using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using Pixel.GEC.Attendance.Configuration;

namespace Pixel.GEC.Attendance.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(AttendanceTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
