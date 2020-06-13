using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pixel.GEC.Attendance.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.HealthChecks
{
    public class AttendanceDbContextHealthCheck : IHealthCheck
    {
        private readonly DatabaseCheckHelper _checkHelper;

        public AttendanceDbContextHealthCheck(DatabaseCheckHelper checkHelper)
        {
            _checkHelper = checkHelper;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (_checkHelper.Exist("db"))
            {
                return Task.FromResult(HealthCheckResult.Healthy("AttendanceDbContext connected to database."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("AttendanceDbContext could not connect to database"));
        }
    }
}
