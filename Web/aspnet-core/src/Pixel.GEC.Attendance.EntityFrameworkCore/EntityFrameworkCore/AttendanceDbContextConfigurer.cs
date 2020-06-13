using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Pixel.GEC.Attendance.EntityFrameworkCore
{
    public static class AttendanceDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AttendanceDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AttendanceDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}