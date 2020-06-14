using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Added_TimeProfileDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeProfileDetails",
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
                    Day = table.Column<int>(nullable: false),
                    ShiftNo = table.Column<int>(nullable: false),
                    TimeProfileId = table.Column<int>(nullable: true),
                    ShiftId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeProfileDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeProfileDetails_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TimeProfileDetails_TimeProfiles_TimeProfileId",
                        column: x => x.TimeProfileId,
                        principalTable: "TimeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeProfileDetails_ShiftId",
                table: "TimeProfileDetails",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeProfileDetails_TimeProfileId",
                table: "TimeProfileDetails",
                column: "TimeProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeProfileDetails");
        }
    }
}
