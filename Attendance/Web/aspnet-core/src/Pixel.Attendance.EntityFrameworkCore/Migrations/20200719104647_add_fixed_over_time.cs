using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_fixed_over_time : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFixedOverTimeAllowed",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixedOverTimeAllowed",
                table: "AbpUsers");
        }
    }
}
