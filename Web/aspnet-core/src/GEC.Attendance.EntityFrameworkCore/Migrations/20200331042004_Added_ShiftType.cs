using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Added_ShiftType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShiftTypes",
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
                    DescriptionEn = table.Column<string>(nullable: true),
                    DescriptionAr = table.Column<string>(nullable: true),
                    NumberOfDuties = table.Column<int>(nullable: false),
                    InScan = table.Column<bool>(nullable: false),
                    OutScan = table.Column<bool>(nullable: false),
                    CrossDay = table.Column<bool>(nullable: false),
                    AlwaysAttend = table.Column<bool>(nullable: false),
                    Open = table.Column<bool>(nullable: false),
                    MaxBoundryTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftTypes");
        }
    }
}
