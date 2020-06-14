using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class add_shifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shifts",
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
                    NameAr = table.Column<string>(nullable: false),
                    NameEn = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    DayOff = table.Column<int>(nullable: false),
                    DayRest = table.Column<int>(nullable: false),
                    TimeIn = table.Column<int>(nullable: false),
                    TimeOut = table.Column<int>(nullable: false),
                    EarlyIn = table.Column<int>(nullable: false),
                    LateIn = table.Column<int>(nullable: false),
                    EarlyOut = table.Column<int>(nullable: false),
                    LateOut = table.Column<int>(nullable: false),
                    TimeInRangeFrom = table.Column<int>(nullable: false),
                    TimeInRangeTo = table.Column<int>(nullable: false),
                    TimeOutRangeFrom = table.Column<int>(nullable: false),
                    TimeOutRangeTo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");
        }
    }
}
