using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class extend_units : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpOrganizationUnits",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ManagerId",
                table: "AbpOrganizationUnits",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpOrganizationUnits_ManagerId",
                table: "AbpOrganizationUnits",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ManagerId",
                table: "AbpOrganizationUnits",
                column: "ManagerId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpOrganizationUnits_AbpUsers_ManagerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropIndex(
                name: "IX_AbpOrganizationUnits_ManagerId",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpOrganizationUnits");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "AbpOrganizationUnits");
        }
    }
}
