using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class modify_users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AbpUsers");

            migrationBuilder.AddColumn<int>(
                name: "NationalityId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OldId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Terminated",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TerminationDate",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TitleId",
                table: "AbpUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_NationalityId",
                table: "AbpUsers",
                column: "NationalityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_Nationalities_NationalityId",
                table: "AbpUsers",
                column: "NationalityId",
                principalTable: "Nationalities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_Nationalities_NationalityId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_NationalityId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "NationalityId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "OldId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Terminated",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "TerminationDate",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "TitleId",
                table: "AbpUsers");

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
