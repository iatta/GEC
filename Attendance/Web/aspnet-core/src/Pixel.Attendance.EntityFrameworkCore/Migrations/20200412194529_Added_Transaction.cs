using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Added_Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
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
                    Time = table.Column<int>(nullable: false),
                    SerialNo = table.Column<int>(nullable: false),
                    TransType = table.Column<int>(nullable: false),
                    Pin = table.Column<int>(nullable: false),
                    Finger = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Manual = table.Column<int>(nullable: false),
                    ScanType = table.Column<int>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false),
                    IsException = table.Column<bool>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    TimeSheetID = table.Column<int>(nullable: false),
                    ProcessInOut = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    KeyType = table.Column<int>(nullable: false),
                    ImportDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }
    }
}
