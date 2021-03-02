using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_no_restrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BreakHours",
                table: "Shifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasBreak",
                table: "Shifts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "NoRestrict",
                table: "Shifts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakHours",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "HasBreak",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "NoRestrict",
                table: "Shifts");
        }
    }
}
