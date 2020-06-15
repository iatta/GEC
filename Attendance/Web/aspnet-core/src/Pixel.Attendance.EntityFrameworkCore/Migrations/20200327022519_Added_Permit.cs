using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Added_Permit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permits",
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
                    DescriptionAr = table.Column<string>(nullable: false),
                    DescriptionEn = table.Column<string>(nullable: false),
                    ToleranceIn = table.Column<int>(nullable: false),
                    ToleranceOut = table.Column<int>(nullable: false),
                    MaxNumberPerDay = table.Column<int>(nullable: false),
                    MaxNumberPerWeek = table.Column<string>(nullable: true),
                    MaxNumberPerMonth = table.Column<int>(nullable: false),
                    TotalHoursPerDay = table.Column<int>(nullable: false),
                    TotalHoursPerWeek = table.Column<int>(nullable: false),
                    TotalHoursPerMonth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permits", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Permits");
        }
    }
}
