using Microsoft.EntityFrameworkCore.Migrations;

namespace Pixel.Attendance.Migrations
{
    public partial class Regenerated_ManualTransaction4271 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "ManualTransactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManualTransactions_MachineId",
                table: "ManualTransactions",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManualTransactions_Machines_MachineId",
                table: "ManualTransactions",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManualTransactions_Machines_MachineId",
                table: "ManualTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ManualTransactions_MachineId",
                table: "ManualTransactions");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "ManualTransactions");
        }
    }
}
