using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Regenerated_LocationCredential7799 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "LocationCredentials");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "LocationCredentials");

            migrationBuilder.AddColumn<string>(
                name: "East",
                table: "LocationCredentials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "North",
                table: "LocationCredentials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "South",
                table: "LocationCredentials",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "West",
                table: "LocationCredentials",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "East",
                table: "LocationCredentials");

            migrationBuilder.DropColumn(
                name: "North",
                table: "LocationCredentials");

            migrationBuilder.DropColumn(
                name: "South",
                table: "LocationCredentials");

            migrationBuilder.DropColumn(
                name: "West",
                table: "LocationCredentials");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "LocationCredentials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "LocationCredentials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
