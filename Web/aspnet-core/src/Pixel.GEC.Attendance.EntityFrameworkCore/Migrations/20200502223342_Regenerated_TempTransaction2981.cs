using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.GEC.Attendance.Migrations
{
    public partial class Regenerated_TempTransaction2981 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TempTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Pin = table.Column<int>(nullable: false),
                    In = table.Column<string>(nullable: true),
                    Timesheet_in = table.Column<DateTime>(nullable: false),
                    Out = table.Column<string>(nullable: true),
                    Timesheet_out = table.Column<DateTime>(nullable: false),
                    LI = table.Column<string>(nullable: true),
                    EO = table.Column<string>(nullable: true),
                    PermissionLI_ID = table.Column<int>(nullable: false),
                    PermissionEO_ID = table.Column<int>(nullable: false),
                    PermissionIO_ID = table.Column<int>(nullable: false),
                    Permission_Text = table.Column<string>(nullable: true),
                    OfficialTask_In_ID = table.Column<int>(nullable: false),
                    OfficialTask_Out_ID = table.Column<int>(nullable: false),
                    OfficialTask_InOut_ID = table.Column<int>(nullable: false),
                    OfficialTask_Text = table.Column<string>(nullable: true),
                    TimeSheet_ExactOut = table.Column<DateTime>(nullable: false),
                    ShiftID = table.Column<int>(nullable: false),
                    OverTime = table.Column<int>(nullable: false),
                    Exception_In = table.Column<int>(nullable: false),
                    Exception_out = table.Column<int>(nullable: false),
                    TimeProfileID = table.Column<int>(nullable: false),
                    ShiftType = table.Column<int>(nullable: false),
                    isNight = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempTransactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TempTransactions");
        }
    }
}
