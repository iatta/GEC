using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_user_shift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_ShiftId",
                table: "AbpUsers",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Shifts_ShiftId",
                table: "AbpUsers",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Shifts_ShiftId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_ShiftId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "AbpUsers");
        }
    }
}
