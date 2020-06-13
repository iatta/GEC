using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.GEC.Attendance.Migrations
{
    public partial class update_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "East",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "North",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "South",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleAr",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "West",
                table: "Locations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "TitleAr",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "West",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
