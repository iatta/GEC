using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class change_hours_to_minutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLateHourPerMonth",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TotalLateHourPerMonthRamadan",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "TotalLateMinutesPerMonth",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalLateMinutesPerMonthRamadan",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalLateMinutesPerMonth",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TotalLateMinutesPerMonthRamadan",
                table: "Shifts");

            migrationBuilder.AddColumn<int>(
                name: "TotalLateHourPerMonth",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalLateHourPerMonthRamadan",
                table: "Shifts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
