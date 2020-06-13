using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.GEC.Attendance.Migrations
{
    public partial class Regenerated_Location3210 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "East",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "North",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "South",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "West",
                table: "Locations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "East",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "North",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "South",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "West",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
