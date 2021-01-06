using Pixel.Attendance.Settings;
using Pixel.Attendance.Attendance;
using Pixel.Attendance.Operation;
using Pixel.Attendance.Operations;
using Pixel.Attendance.Setting;
using Abp.IdentityServer4;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pixel.Attendance.Authorization.Roles;
using Pixel.Attendance.Authorization.Users;
using Pixel.Attendance.Chat;
using Pixel.Attendance.Editions;
using Pixel.Attendance.Friendships;
using Pixel.Attendance.MultiTenancy;
using Pixel.Attendance.MultiTenancy.Accounting;
using Pixel.Attendance.MultiTenancy.Payments;
using Pixel.Attendance.Storage;
using Pixel.Attendance.ReportsModel;
using Pixel.Attendance.Extended;

namespace Pixel.Attendance.EntityFrameworkCore
{
    public class AttendanceDbContext : AbpZeroDbContext<Tenant, Role, User, AttendanceDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<RamadanDate> RamadanDates { get; set; }

        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }

        public virtual DbSet<OverrideShift> OverrideShifts { get; set; }

        public virtual DbSet<EmployeeTempTransfer> EmployeeTempTransfers { get; set; }

        public virtual DbSet<OrganizationLocation> OrganizationLocations { get; set; }

        public virtual DbSet<ProjectLocation> ProjectLocations { get; set; }

        public virtual DbSet<LocationMachine> LocationMachines { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<UserTimeSheetApprove> UserTimeSheetApproves { get; set; }

        public virtual DbSet<Beacon> Beacons { get; set; }

        public virtual DbSet<UserShift> UserShifts { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<MobileWebPage> MobileWebPages { get; set; }

        public virtual DbSet<Nationality> Nationalities { get; set; }

        public virtual DbSet<EmployeeWarning> EmployeeWarnings { get; set; }

        public virtual DbQuery<InOutReportOutputCore> InOutReportOutputCore { get; set; }
        public virtual DbQuery<FingerReportCore> FingerReportCore { get; set; }
        public virtual DbQuery<ForgetInOutCore> ForgetInOutCore { get; set; }
        public virtual DbQuery<PermitReportCore> PermitReportCore { get; set; }
        public virtual DbQuery<EmployeeReportCore> EmployeeReportCore { get; set; }

        

        public virtual DbSet<TempTransaction> TempTransactions { get; set; }

        public virtual DbSet<ManualTransaction> ManualTransactions { get; set; }

        public virtual DbSet<Tran> Trans { get; set; }

        public virtual DbSet<UserDevice> UserDevices { get; set; }

        public virtual DbSet<MobileTransaction> MobileTransactions { get; set; }

        public virtual DbSet<EmployeeAbsence> EmployeeAbsences { get; set; }

        public virtual DbSet<LocationCredential> LocationCredentials { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<EmployeeOfficialTaskDetail> EmployeeOfficialTaskDetails { get; set; }

        public virtual DbSet<EmployeePermit> EmployeePermits { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        public virtual DbSet<Machine> Machines { get; set; }

        public virtual DbSet<EmployeeOfficialTask> EmployeeOfficialTasks { get; set; }

        public virtual DbSet<OfficialTaskType> OfficialTaskTypes { get; set; }

        public virtual DbSet<TimeProfileDetail> TimeProfileDetails { get; set; }

        public virtual DbSet<TimeProfile> TimeProfiles { get; set; }

        public virtual DbSet<ShiftTypeDetail> ShiftTypeDetails { get; set; }

        public virtual DbSet<ShiftType> ShiftTypes { get; set; }

        public virtual DbSet<Shift> Shifts { get; set; }

        public virtual DbSet<WarningType> WarningTypes { get; set; }

        public virtual DbSet<EmployeeVacation> EmployeeVacations { get; set; }

        public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }

        public virtual DbSet<Permit> Permits { get; set; }

        public virtual DbSet<TypesOfPermit> TypesOfPermits { get; set; }

        public virtual DbSet<Holiday> Holidays { get; set; }

        public virtual DbSet<LeaveType> LeaveTypes { get; set; }

        public virtual DbSet<JobTitle> JobTitles { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public DbSet<PermitType> PermitTypes { get; set; }

        //public DbSet<ProjectUser> ProjectUsers { get; set; }

        public DbSet<OrganizationUnitExtended> AbpOrganizationUnits { get; set; }

        

        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
            : base(options)
        {

        }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Ignore<InOutReportOutputCore>();

            modelBuilder.Entity<ShiftType>()
                .HasMany(c => c.ShiftTypeDetails)
                .WithOne(e => e.ShiftTypeFk);

            modelBuilder.Entity<Location>()
               .HasMany(c => c.LocationCredentials)
               .WithOne(e => e.LocationFk);


            modelBuilder.Entity<Location>()
               .HasMany(c => c.Machines)
               .WithOne(e => e.LocationFk);


            modelBuilder.Entity<Project>()
               .HasMany(c => c.Locations)
               .WithOne(e => e.ProjectFk);


            modelBuilder.Entity<OrganizationUnitExtended>()
               .HasMany(c => c.Locations)
               .WithOne(e => e.OrganizationUnitFk);




            modelBuilder.Entity<ProjectMachine>().ToTable("ProjectMachines");

            modelBuilder.Entity<ProjectMachine>().HasKey(ujr => new { ujr.MachineId, ujr.ProjectId });

            modelBuilder.Entity<ProjectMachine>().HasOne(ujr => ujr.Machine).WithMany(u => u.Projects)
                                                         .HasForeignKey(ujr => ujr.MachineId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMachine>().HasOne(ujr => ujr.Project).WithMany(j => j.Machines)
                                                         .HasForeignKey(ujr => ujr.ProjectId).OnDelete(DeleteBehavior.Restrict);




            modelBuilder.Entity<ProjectUser>().ToTable("ProjectUsers");


            modelBuilder.Entity<ProjectUser>().HasKey(ujr => new { ujr.UserId, ujr.ProjectId });

            modelBuilder.Entity<ProjectUser>().HasOne(ujr => ujr.User).WithMany(u => u.Projects)
                                                         .HasForeignKey(ujr => ujr.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectUser>().HasOne(ujr => ujr.Project).WithMany(j => j.Users)
                                                         .HasForeignKey(ujr => ujr.ProjectId).OnDelete(DeleteBehavior.Restrict);







            //modelBuilder.Entity<ProjectUser>()
            //.HasKey(c => new { c.ProjectId, c.UserId });

            modelBuilder.Entity<PermitType>().ToTable("PermitTypes");

            

            modelBuilder.Entity<PermitType>()
              .HasKey(c => new { c.PermitId, c.TypesOfPermitId });

            modelBuilder.Entity<UserLocation>().ToTable("UserLocations");

            modelBuilder.Entity<UserLocation>()
              .HasKey(c => new { c.UserId, c.LocationId });


            modelBuilder.Entity<EmployeeOfficialTaskDetail>().ToTable("EmployeeOfficialTaskDetails");

            modelBuilder.Entity<EmployeeOfficialTaskDetail>()
            .HasKey(c => new { c.UserId, c.EmployeeOfficialTaskId });



            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
