using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Regenerated_MobileWebPage2520 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Neme",
                table: "MobileWebPages");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MobileWebPages",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MobileWebPages");

            migrationBuilder.AddColumn<string>(
                name: "Neme",
                table: "MobileWebPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
