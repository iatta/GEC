using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class change_permit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Permits",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeductType",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EarlyOut",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "FullDay",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "InOut",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LateIn",
                table: "Permits",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OffShift",
                table: "Permits",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "DeductType",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "EarlyOut",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "FullDay",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "InOut",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "LateIn",
                table: "Permits");

            migrationBuilder.DropColumn(
                name: "OffShift",
                table: "Permits");
        }
    }
}
