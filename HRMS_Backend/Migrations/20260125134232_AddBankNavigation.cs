using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBankNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinancialDatas_BankBranchId",
                table: "EmployeeFinancialDatas",
                column: "BankBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinancialDatas_BankId",
                table: "EmployeeFinancialDatas",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFinancialDatas_BankBranches_BankBranchId",
                table: "EmployeeFinancialDatas",
                column: "BankBranchId",
                principalTable: "BankBranches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFinancialDatas_Banks_BankId",
                table: "EmployeeFinancialDatas",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFinancialDatas_BankBranches_BankBranchId",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFinancialDatas_Banks_BankId",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFinancialDatas_BankBranchId",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFinancialDatas_BankId",
                table: "EmployeeFinancialDatas");
        }
    }
}
