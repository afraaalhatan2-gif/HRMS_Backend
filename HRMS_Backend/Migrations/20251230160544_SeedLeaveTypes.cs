using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedLeaveTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveType_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType");

            migrationBuilder.RenameTable(
                name: "LeaveType",
                newName: "LeaveTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes",
                column: "Id");

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "سنوية" },
                    { 2, true, "مرضية" },
                    { 3, true, "بدون مرتب" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                table: "LeaveRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeaveTypes",
                table: "LeaveTypes");

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "LeaveTypes",
                newName: "LeaveType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeaveType",
                table: "LeaveType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_LeaveType_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId",
                principalTable: "LeaveType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
