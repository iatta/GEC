using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class add_manager_identifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "AbpUsers");
        }
    }
}
