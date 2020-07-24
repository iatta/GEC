using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Regenerated_UserTimeSheetApprove5279 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ProjectManagerApprove",
                table: "UserTimeSheetApproves",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ProjectManagerId",
                table: "UserTimeSheetApproves",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimeSheetApproves_ProjectManagerId",
                table: "UserTimeSheetApproves",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimeSheetApproves_AbpUsers_ProjectManagerId",
                table: "UserTimeSheetApproves",
                column: "ProjectManagerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimeSheetApproves_AbpUsers_ProjectManagerId",
                table: "UserTimeSheetApproves");

            migrationBuilder.DropIndex(
                name: "IX_UserTimeSheetApproves_ProjectManagerId",
                table: "UserTimeSheetApproves");

            migrationBuilder.DropColumn(
                name: "ProjectManagerApprove",
                table: "UserTimeSheetApproves");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "UserTimeSheetApproves");
        }
    }
}
