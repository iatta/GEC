using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Regenerated_UserTimeSheetApprove9134 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "UserTimeSheetApproves",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimeSheetApproves_ProjectId",
                table: "UserTimeSheetApproves",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimeSheetApproves_Projects_ProjectId",
                table: "UserTimeSheetApproves",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimeSheetApproves_Projects_ProjectId",
                table: "UserTimeSheetApproves");

            migrationBuilder.DropIndex(
                name: "IX_UserTimeSheetApproves_ProjectId",
                table: "UserTimeSheetApproves");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "UserTimeSheetApproves");
        }
    }
}
