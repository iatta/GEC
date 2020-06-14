using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class face_map_change_image : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iamge",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "Iamge",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
