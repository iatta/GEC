using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.GEC.Attendance.Migrations
{
    public partial class Added_Tran : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trans",
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
                    Scan1 = table.Column<string>(nullable: true),
                    Scan2 = table.Column<string>(nullable: true),
                    Scan3 = table.Column<string>(nullable: true),
                    Scan4 = table.Column<string>(nullable: true),
                    Scan5 = table.Column<string>(nullable: true),
                    Scan6 = table.Column<string>(nullable: true),
                    Scan8 = table.Column<string>(nullable: true),
                    ScanLocation1 = table.Column<string>(nullable: true),
                    ScanLocation2 = table.Column<string>(nullable: true),
                    ScanLocation3 = table.Column<string>(nullable: true),
                    ScanLocation4 = table.Column<string>(nullable: true),
                    ScanLocation5 = table.Column<string>(nullable: true),
                    ScanLocation6 = table.Column<string>(nullable: true),
                    ScanLocation7 = table.Column<string>(nullable: true),
                    ScanLocation8 = table.Column<string>(nullable: true),
                    HasHoliday = table.Column<bool>(nullable: false),
                    HasVacation = table.Column<bool>(nullable: false),
                    HasOffDay = table.Column<bool>(nullable: false),
                    IsAbsent = table.Column<bool>(nullable: false),
                    LeaveCode = table.Column<string>(nullable: true),
                    DesignationID = table.Column<int>(nullable: false),
                    LeaveRemark = table.Column<string>(nullable: true),
                    NoShifts = table.Column<int>(nullable: false),
                    ShiftName = table.Column<string>(nullable: true),
                    ScanManual1 = table.Column<bool>(nullable: false),
                    ScanManual2 = table.Column<bool>(nullable: false),
                    ScanManual3 = table.Column<bool>(nullable: false),
                    ScanManual4 = table.Column<bool>(nullable: false),
                    ScanManual5 = table.Column<bool>(nullable: false),
                    ScanManual6 = table.Column<bool>(nullable: false),
                    ScanManual7 = table.Column<bool>(nullable: false),
                    ScanManual8 = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trans_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trans_UserId",
                table: "Trans",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trans");
        }
    }
}
