using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeFinancialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAdministrativeData_Employee_EmployeeId",
                table: "EmployeeAdministrativeData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAdministrativeData",
                table: "EmployeeAdministrativeData");

            migrationBuilder.RenameTable(
                name: "EmployeeAdministrativeData",
                newName: "EmployeeAdministrativeDatas");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "EmployeeFinancialDatas",
                newName: "BasicSalary");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAdministrativeData_EmployeeId",
                table: "EmployeeAdministrativeDatas",
                newName: "IX_EmployeeAdministrativeDatas_EmployeeId");

            migrationBuilder.AddColumn<decimal>(
                name: "Allowances",
                table: "EmployeeFinancialDatas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Deductions",
                table: "EmployeeFinancialDatas",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAdministrativeDatas",
                table: "EmployeeAdministrativeDatas",
                column: "Id");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[] { 18, 4, 5 });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeFinancialDatas_EmployeeId",
                table: "EmployeeFinancialDatas",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAdministrativeDatas_Employee_EmployeeId",
                table: "EmployeeAdministrativeDatas",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeFinancialDatas_Employee_EmployeeId",
                table: "EmployeeFinancialDatas",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAdministrativeDatas_Employee_EmployeeId",
                table: "EmployeeAdministrativeDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeFinancialDatas_Employee_EmployeeId",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeFinancialDatas_EmployeeId",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeAdministrativeDatas",
                table: "EmployeeAdministrativeDatas");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DropColumn(
                name: "Allowances",
                table: "EmployeeFinancialDatas");

            migrationBuilder.DropColumn(
                name: "Deductions",
                table: "EmployeeFinancialDatas");

            migrationBuilder.RenameTable(
                name: "EmployeeAdministrativeDatas",
                newName: "EmployeeAdministrativeData");

            migrationBuilder.RenameColumn(
                name: "BasicSalary",
                table: "EmployeeFinancialDatas",
                newName: "Salary");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeAdministrativeDatas_EmployeeId",
                table: "EmployeeAdministrativeData",
                newName: "IX_EmployeeAdministrativeData_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeAdministrativeData",
                table: "EmployeeAdministrativeData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAdministrativeData_Employee_EmployeeId",
                table: "EmployeeAdministrativeData",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
