using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class Regenerated_OfficialTaskType4675 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfficialTaskTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    TypeIn = table.Column<bool>(nullable: false),
                    TypeOut = table.Column<bool>(nullable: false),
                    TypeInOut = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialTaskTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialTaskTypes");
        }
    }
}
