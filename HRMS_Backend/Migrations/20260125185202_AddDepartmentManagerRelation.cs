using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentManagerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_ManagerId",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Employee",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_ManagerId",
                table: "Employee",
                newName: "IX_Employee_EmployeeId");

            migrationBuilder.AddColumn<int>(
                name: "ManagerEmployeeId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerEmployeeId",
                table: "Departments",
                column: "ManagerEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Employee_ManagerEmployeeId",
                table: "Departments",
                column: "ManagerEmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_EmployeeId",
                table: "Employee",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Employee_ManagerEmployeeId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_EmployeeId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ManagerEmployeeId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ManagerEmployeeId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "Employee",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_EmployeeId",
                table: "Employee",
                newName: "IX_Employee_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_ManagerId",
                table: "Employee",
                column: "ManagerId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
