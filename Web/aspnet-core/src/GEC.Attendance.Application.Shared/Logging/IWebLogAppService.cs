using Abp.Application.Services;
using GEC.Attendance.Dto;
using GEC.Attendance.Logging.Dto;

namespace GEC.Attendance.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
