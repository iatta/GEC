using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Regenerated_EmployeeWarning6749 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeReportCore",
                columns: table => new
                {
                    EmpId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpName = table.Column<string>(nullable: true),
                    AbsentCount = table.Column<int>(nullable: false),
                    AttendanceCount = table.Column<int>(nullable: false),
                    AbsentContinusDays = table.Column<int>(nullable: false),
                    DaysCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeReportCore", x => x.EmpId);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWarnings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    WarningTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWarnings_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWarnings_WarningTypes_WarningTypeId",
                        column: x => x.WarningTypeId,
                        principalTable: "WarningTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermitReportCore",
                columns: table => new
                {
                    PermitId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    TypeEn = table.Column<string>(nullable: true),
                    TypeAr = table.Column<string>(nullable: true),
                    StatusEn = table.Column<string>(nullable: true),
                    StatusAr = table.Column<string>(nullable: true),
                    KindAr = table.Column<string>(nullable: true),
                    KindEn = table.Column<string>(nullable: true),
                    ToTime = table.Column<int>(nullable: false),
                    FromTime = table.Column<int>(nullable: false),
                    EmpId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitReportCore", x => x.PermitId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWarnings_UserId",
                table: "EmployeeWarnings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWarnings_WarningTypeId",
                table: "EmployeeWarnings",
                column: "WarningTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeReportCore");

            migrationBuilder.DropTable(
                name: "EmployeeWarnings");

            migrationBuilder.DropTable(
                name: "PermitReportCore");
        }
    }
}
