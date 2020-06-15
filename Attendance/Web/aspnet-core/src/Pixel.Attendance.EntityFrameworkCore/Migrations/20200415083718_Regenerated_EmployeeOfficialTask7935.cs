using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Regenerated_EmployeeOfficialTask7935 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOfficialTasks_AbpUsers_UserId",
                table: "EmployeeOfficialTasks");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeOfficialTasks_UserId",
                table: "EmployeeOfficialTasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EmployeeOfficialTasks");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "EmployeeOfficialTasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "EmployeeOfficialTasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "EmployeeOfficialTasks");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "EmployeeOfficialTasks");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "EmployeeOfficialTasks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfficialTasks_UserId",
                table: "EmployeeOfficialTasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOfficialTasks_AbpUsers_UserId",
                table: "EmployeeOfficialTasks",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
