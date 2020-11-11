using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class update_shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasRamadanSetting",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDayRestCalculated",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFlexible",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOneFingerprint",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTwoFingerprint",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TimeInRamadan",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeOutRamadan",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalHoursPerDay",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalHoursPerDayRamadan",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalLateHourPerMonth",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRamadanSetting",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "IsDayRestCalculated",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "IsFlexible",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "IsOneFingerprint",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "IsTwoFingerprint",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TimeInRamadan",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TimeOutRamadan",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TotalHoursPerDay",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TotalHoursPerDayRamadan",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "TotalLateHourPerMonth",
                table: "Shifts");
        }
    }
}
