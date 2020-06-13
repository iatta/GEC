using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.GEC.Attendance.Migrations
{
    public partial class face_map : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FaceMap",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Iamge",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFaceRegistered",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceMap",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Iamge",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "IsFaceRegistered",
                table: "AbpUsers");
        }
    }
}
