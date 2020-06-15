using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class extend_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CivilId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Device",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FingerCode",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FingerPassword",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameEn",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobTitleId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "AbpUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone_1",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telephone_2",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_JobTitleId",
                table: "AbpUsers",
                column: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_JobTitles_JobTitleId",
                table: "AbpUsers",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_JobTitles_JobTitleId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_JobTitleId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "CivilId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Device",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "FingerCode",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "FingerPassword",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "FullNameEn",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "JobTitleId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Telephone_1",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Telephone_2",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AbpUsers");
        }
    }
}
