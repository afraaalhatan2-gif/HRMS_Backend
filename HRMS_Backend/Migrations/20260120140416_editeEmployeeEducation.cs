using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class editeEmployeeEducation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEducation_Employee_EmployeeId",
                table: "EmployeeEducation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEducation",
                table: "EmployeeEducation");

            migrationBuilder.RenameTable(
                name: "EmployeeEducation",
                newName: "EmployeeEducations");

            migrationBuilder.RenameColumn(
                name: "Institution",
                table: "EmployeeEducations",
                newName: "University");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeEducation_EmployeeId",
                table: "EmployeeEducations",
                newName: "IX_EmployeeEducations_EmployeeId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmployeeEducations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEducations",
                table: "EmployeeEducations",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "PermissionName" },
                values: new object[,]
                {
                    { 11, "AddOwnEducation" },
                    { 12, "EditOwnEducation" },
                    { 13, "ManageEmployeeEducation" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 13, 11, 1 },
                    { 14, 12, 1 },
                    { 15, 13, 1 },
                    { 16, 11, 5 },
                    { 17, 12, 5 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEducations_Employee_EmployeeId",
                table: "EmployeeEducations",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeEducations_Employee_EmployeeId",
                table: "EmployeeEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeEducations",
                table: "EmployeeEducations");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmployeeEducations");

            migrationBuilder.RenameTable(
                name: "EmployeeEducations",
                newName: "EmployeeEducation");

            migrationBuilder.RenameColumn(
                name: "University",
                table: "EmployeeEducation",
                newName: "Institution");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeEducations_EmployeeId",
                table: "EmployeeEducation",
                newName: "IX_EmployeeEducation_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeEducation",
                table: "EmployeeEducation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeEducation_Employee_EmployeeId",
                table: "EmployeeEducation",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
