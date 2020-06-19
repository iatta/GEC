using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class change_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Pin",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Pin",
                table: "Transactions",
                column: "Pin");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AbpUsers_Pin",
                table: "Transactions",
                column: "Pin",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AbpUsers_Pin",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_Pin",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "Pin",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
