using Microsoft.Extensions.Configuration;

namespace Pixel.Attendance.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
