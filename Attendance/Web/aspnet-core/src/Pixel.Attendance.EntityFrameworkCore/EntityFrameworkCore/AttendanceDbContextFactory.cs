using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pixel.Attendance.Configuration;
using Pixel.Attendance.Web;

namespace Pixel.Attendance.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AttendanceDbContextFactory : IDesignTimeDbContextFactory<AttendanceDbContext>
    {
        public AttendanceDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AttendanceDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            AttendanceDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AttendanceConsts.ConnectionStringName));

            return new AttendanceDbContext(builder.Options);
        }
    }
}