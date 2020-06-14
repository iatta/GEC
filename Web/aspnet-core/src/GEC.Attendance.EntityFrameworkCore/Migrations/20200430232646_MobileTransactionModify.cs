using Microsoft.EntityFrameworkCore.Migrations;

namespace GEC.Attendance.Migrations
{
    public partial class MobileTransactionModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FingerCode",
                table: "MobileTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "TransType",
                table: "MobileTransactions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmpCode",
                table: "MobileTransactions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpCode",
                table: "MobileTransactions");

            migrationBuilder.AlterColumn<int>(
                name: "TransType",
                table: "MobileTransactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FingerCode",
                table: "MobileTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
