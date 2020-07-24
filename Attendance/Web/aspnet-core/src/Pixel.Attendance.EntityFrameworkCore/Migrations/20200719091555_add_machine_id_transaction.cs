using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class add_machine_id_transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "Transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_MachineId",
                table: "Transactions",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Machines_MachineId",
                table: "Transactions",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Machines_MachineId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_MachineId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Transactions");
        }
    }
}
