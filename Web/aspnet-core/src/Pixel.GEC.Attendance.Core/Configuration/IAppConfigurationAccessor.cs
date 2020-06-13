using Microsoft.Extensions.Configuration;

namespace Pixel.GEC.Attendance.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
