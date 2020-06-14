using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Regenerated_LocationCredential9312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "LocationCredentials",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "LocationCredentials",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "North",
                table: "LocationCredentials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "South",
                table: "LocationCredentials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "West",
                table: "LocationCredentials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
