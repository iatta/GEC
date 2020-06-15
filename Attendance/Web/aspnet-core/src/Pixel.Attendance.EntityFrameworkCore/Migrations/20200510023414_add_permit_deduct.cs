using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_permit_deduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeducted",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FingerReportCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Scan1 = table.Column<string>(nullable: true),
                    Scan2 = table.Column<string>(nullable: true),
                    Scan3 = table.Column<string>(nullable: true),
                    Scan4 = table.Column<string>(nullable: true),
                    Scan5 = table.Column<string>(nullable: true),
                    Scan6 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FingerReportCore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForgetInOutCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceDate = table.Column<DateTime>(nullable: true),
                    AttendanceInStr = table.Column<string>(nullable: true),
                    AttendanceOutStr = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    UnitName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForgetInOutCore", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InOutReportOutputCore",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceDate = table.Column<DateTime>(nullable: true),
                    AttendanceIn = table.Column<DateTime>(nullable: true),
                    AttendanceInStr = table.Column<string>(nullable: true),
                    AttendanceLateIn = table.Column<long>(nullable: true),
                    AttendanceOut = table.Column<DateTime>(nullable: true),
                    AttendanceOutStr = table.Column<string>(nullable: true),
                    AttendanceEarlyOut = table.Column<long>(nullable: true),
                    IsOff = table.Column<bool>(nullable: true),
                    IsHoliday = table.Column<bool>(nullable: true),
                    IsVacation = table.Column<bool>(nullable: true),
                    VacationCode = table.Column<string>(nullable: true),
                    IsAbsent = table.Column<int>(nullable: true),
                    IsPermissionOff = table.Column<bool>(nullable: true),
                    IsShiftOff = table.Column<bool>(nullable: true),
                    TimeProfileId = table.Column<long>(nullable: true),
                    EmpId = table.Column<long>(nullable: true),
                    TotalHours = table.Column<int>(nullable: true),
                    MissedScan = table.Column<bool>(nullable: true),
                    TotalHoursStr = table.Column<string>(nullable: true),
                    ShiftNo = table.Column<long>(nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    UserFullName = table.Column<string>(nullable: true),
                    EmpName = table.Column<string>(nullable: true),
                    ShiftId = table.Column<long>(nullable: true),
                    UnitId = table.Column<long>(nullable: true),
                    TotalMin = table.Column<int>(nullable: true),
                    TotalPerMin = table.Column<long>(nullable: true),
                    ForgetOut = table.Column<int>(nullable: true),
                    ForgetIn = table.Column<int>(nullable: true),
                    OverTime = table.Column<long>(nullable: true),
                    ShiftCode = table.Column<string>(nullable: true),
                    CivilId = table.Column<string>(nullable: true),
                    JobTitleName = table.Column<string>(nullable: true),
                    MainDisplayName = table.Column<string>(nullable: true),
                    MainUnitId = table.Column<long>(nullable: true),
                    LeaveTypeNameE = table.Column<string>(nullable: true),
                    LeaveTypeNameA = table.Column<string>(nullable: true),
                    PermitCode = table.Column<string>(nullable: true),
                    PermitCodeEn = table.Column<string>(nullable: true),
                    PermitIsDeducted = table.Column<string>(nullable: true),
                    OfficialTaskTypeNameAr = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    AttendanceOfficialTaskText = table.Column<string>(nullable: true),
                    AbsentsCount = table.Column<int>(nullable: true),
                    VacationsCount = table.Column<int>(nullable: true),
                    NoInCount = table.Column<int>(nullable: true),
                    NoOutCount = table.Column<int>(nullable: true),
                    OffDaysCount = table.Column<int>(nullable: true),
                    PermitCount = table.Column<int>(nullable: true),
                    PermitTotal = table.Column<int>(nullable: true),
                    ShiftNameAr = table.Column<string>(nullable: true),
                    ShiftNameEn = table.Column<string>(nullable: true),
                    HolidayName = table.Column<string>(nullable: true),
                    FingerCode = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InOutReportOutputCore", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FingerReportCore");

            migrationBuilder.DropTable(
                name: "ForgetInOutCore");

            migrationBuilder.DropTable(
                name: "InOutReportOutputCore");

            migrationBuilder.DropColumn(
                name: "IsDeducted",
                table: "Permits");
        }
    }
}
