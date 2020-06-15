using Abp.Application.Services;
using Pixel.Attendance.Dto;
using Pixel.Attendance.Logging.Dto;

namespace Pixel.Attendance.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
