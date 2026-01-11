using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddLeaveRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDays = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmploymentStatusId",
                table: "Employee",
                column: "EmploymentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_JobGradeId",
                table: "Employee",
                column: "JobGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_JobTitleId",
                table: "Employee",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_MaritalStatusId",
                table: "Employee",
                column: "MaritalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_WorkLocationId",
                table: "Employee",
                column: "WorkLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                table: "Employee",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmploymentStatuses_EmploymentStatusId",
                table: "Employee",
                column: "EmploymentStatusId",
                principalTable: "EmploymentStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_JobGrades_JobGradeId",
                table: "Employee",
                column: "JobGradeId",
                principalTable: "JobGrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_JobTitles_JobTitleId",
                table: "Employee",
                column: "JobTitleId",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_MaritalStatuses_MaritalStatusId",
                table: "Employee",
                column: "MaritalStatusId",
                principalTable: "MaritalStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_WorkLocations_WorkLocationId",
                table: "Employee",
                column: "WorkLocationId",
                principalTable: "WorkLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Departments_DepartmentId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmploymentStatuses_EmploymentStatusId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_JobGrades_JobGradeId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_JobTitles_JobTitleId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_MaritalStatuses_MaritalStatusId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_WorkLocations_WorkLocationId",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "LeaveType");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmploymentStatusId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_JobGradeId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_JobTitleId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_MaritalStatusId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_WorkLocationId",
                table: "Employee");
        }
    }
}
