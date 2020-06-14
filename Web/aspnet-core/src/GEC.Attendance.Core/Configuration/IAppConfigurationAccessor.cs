using Microsoft.Extensions.Configuration;

namespace GEC.Attendance.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
