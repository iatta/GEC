using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_hr_approve : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HrApprove",
                table: "Transactions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HrApprove",
                table: "Transactions");
        }
    }
}
