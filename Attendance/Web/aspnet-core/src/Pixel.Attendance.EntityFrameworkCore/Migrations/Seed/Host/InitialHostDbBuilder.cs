using Pixel.Attendance.EntityFrameworkCore;

namespace Pixel.Attendance.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly AttendanceDbContext _context;

        public InitialHostDbBuilder(AttendanceDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
