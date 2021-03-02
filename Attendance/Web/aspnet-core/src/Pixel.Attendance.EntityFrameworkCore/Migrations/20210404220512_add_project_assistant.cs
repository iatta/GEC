using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_project_assistant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ManagerAssistantId",
                table: "Projects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskTypeId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerAssistantId",
                table: "Projects",
                column: "ManagerAssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_TaskTypeId",
                table: "AbpUsers",
                column: "TaskTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_TaskTypes_TaskTypeId",
                table: "AbpUsers",
                column: "TaskTypeId",
                principalTable: "TaskTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AbpUsers_ManagerAssistantId",
                table: "Projects",
                column: "ManagerAssistantId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_TaskTypes_TaskTypeId",
                table: "AbpUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AbpUsers_ManagerAssistantId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ManagerAssistantId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_TaskTypeId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "ManagerAssistantId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TaskTypeId",
                table: "AbpUsers");
        }
    }
}
