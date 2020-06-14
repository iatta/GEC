using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Added_EmployeeOfficialTaskDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeOfficialTaskDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    EmployeeOfficialTaskId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeOfficialTaskDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeOfficialTaskDetails_EmployeeOfficialTasks_EmployeeOfficialTaskId",
                        column: x => x.EmployeeOfficialTaskId,
                        principalTable: "EmployeeOfficialTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeOfficialTaskDetails_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfficialTaskDetails_EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails",
                column: "EmployeeOfficialTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfficialTaskDetails_UserId",
                table: "EmployeeOfficialTaskDetails",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeOfficialTaskDetails");
        }
    }
}
