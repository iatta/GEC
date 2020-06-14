using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class add_permit_types : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PermitTypes",
                columns: table => new
                {
                    PermitId = table.Column<int>(nullable: false),
                    TypesOfPermitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitTypes", x => new { x.PermitId, x.TypesOfPermitId });
                    table.ForeignKey(
                        name: "FK_PermitTypes_Permits_PermitId",
                        column: x => x.PermitId,
                        principalTable: "Permits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermitTypes_TypesOfPermits_TypesOfPermitId",
                        column: x => x.TypesOfPermitId,
                        principalTable: "TypesOfPermits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermitTypes_TypesOfPermitId",
                table: "PermitTypes",
                column: "TypesOfPermitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermitTypes");
        }
    }
}
