using Abp.Application.Services;
using Pixel.GEC.Attendance.Dto;
using Pixel.GEC.Attendance.Logging.Dto;

namespace Pixel.GEC.Attendance.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
