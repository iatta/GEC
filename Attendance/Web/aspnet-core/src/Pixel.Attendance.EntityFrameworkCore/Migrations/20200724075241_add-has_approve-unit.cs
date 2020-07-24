using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class addhas_approveunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasApprove",
                table: "AbpOrganizationUnits",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasApprove",
                table: "AbpOrganizationUnits");
        }
    }
}
