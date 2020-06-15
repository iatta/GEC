using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class change_emp_official_task_detail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_EmployeeOfficialTasks_EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_AbpUsers_UserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeOfficialTaskDetails",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeOfficialTaskDetails_UserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "EmployeeOfficialTaskDetails",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeOfficialTaskDetails",
                table: "EmployeeOfficialTaskDetails",
                columns: new[] { "UserId", "EmployeeOfficialTaskId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_EmployeeOfficialTasks_EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails",
                column: "EmployeeOfficialTaskId",
                principalTable: "EmployeeOfficialTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_AbpUsers_UserId",
                table: "EmployeeOfficialTaskDetails",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_EmployeeOfficialTasks_EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_AbpUsers_UserId",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeOfficialTaskDetails",
                table: "EmployeeOfficialTaskDetails");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "EmployeeOfficialTaskDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "EmployeeOfficialTaskDetails",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "EmployeeOfficialTaskDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "EmployeeOfficialTaskDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "EmployeeOfficialTaskDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "EmployeeOfficialTaskDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployeeOfficialTaskDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "EmployeeOfficialTaskDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "EmployeeOfficialTaskDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeOfficialTaskDetails",
                table: "EmployeeOfficialTaskDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeOfficialTaskDetails_UserId",
                table: "EmployeeOfficialTaskDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_EmployeeOfficialTasks_EmployeeOfficialTaskId",
                table: "EmployeeOfficialTaskDetails",
                column: "EmployeeOfficialTaskId",
                principalTable: "EmployeeOfficialTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeOfficialTaskDetails_AbpUsers_UserId",
                table: "EmployeeOfficialTaskDetails",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
