using System.Collections.Generic;
using HRMS_Backend.Data;
using HRMS_Backend.Enums;
using HRMS_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Backend.Data
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
            { 
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MaritalStatus>().HasData(
    new MaritalStatus { Id = 1, Name = "أعزب" },
    new MaritalStatus { Id = 2, Name = "متزوج" },
    new MaritalStatus { Id = 3, Name = "مطلق" },
    new MaritalStatus { Id = 4, Name = "ارمل" }
);

            modelBuilder.Entity<JobTitle>().HasData(
                new JobTitle { Id = 1, Name = "موظف" },
                new JobTitle { Id = 2, Name = "مشرف" },
                new JobTitle { Id = 3, Name = "مدير" }
            );

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "SuperAdmin" },
                new Role { Id = 2, RoleName = "مدير إدارة" },
                new Role { Id = 3, RoleName = "مدير إدارة فرعية" },
                new Role { Id = 4, RoleName = "مدير قسم" },
                new Role { Id = 5, RoleName = "موظف" }
            );
            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, PermissionName = "AddEmployee" },
                new Permission { Id = 2, PermissionName = "EditEmployee" },
                new Permission { Id = 3, PermissionName = "DeleteEmployee" },
                new Permission { Id = 4, PermissionName = "ViewEmployee" },
                new Permission { Id = 5, PermissionName = "ApproveLeave" },
                new Permission { Id = 6, PermissionName = "SubmitLeave" },
                new Permission { Id = 7, PermissionName = "SubmitComplaint" },
                new Permission { Id = 8, PermissionName = "ViewComplaints" },
                new Permission { Id = 9, PermissionName = "AssignTask" },
                new Permission { Id = 10, PermissionName = "ViewDepartmentEmployees" }
            );
            // SuperAdmin gets ALL permissions
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
                new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
                new RolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
                new RolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
                new RolePermission { Id = 5, RoleId = 1, PermissionId = 5 },
                new RolePermission { Id = 6, RoleId = 1, PermissionId = 6 },
                new RolePermission { Id = 7, RoleId = 1, PermissionId = 7 },
                new RolePermission { Id = 8, RoleId = 1, PermissionId = 8 },
                new RolePermission { Id = 9, RoleId = 1, PermissionId = 9 },
                new RolePermission { Id = 10, RoleId = 1, PermissionId = 10 },

              //  موظف → SubmitLeave فقط
    new RolePermission { Id = 11, RoleId = 5, PermissionId = 6 },

     // مدير قسم → ApproveLeave
    new RolePermission { Id = 12, RoleId = 4, PermissionId = 5 }

            );
            modelBuilder.Entity<LeaveTypes>().HasData(
                new LeaveTypes
                {
                    Id = 1,
                    اسم_الاجازة = "إجازة سنوية",
                    مخصومة_من_الرصيد = true,
                    تحتاج_نموذج = false,
                    مفعلة = true
                },
                new LeaveTypes
                {
                    Id = 2,
                    اسم_الاجازة = "إجازة مرضية",
                    مخصومة_من_الرصيد = true,
                    تحتاج_نموذج = true,
                    مفعلة = true
                },
                new LeaveTypes
                {
                    Id = 3,
                    اسم_الاجازة = "إجازة حج",
                    مخصومة_من_الرصيد = false,
                    تحتاج_نموذج = true,
                    مفعلة = true
                },
                new LeaveTypes
                {
                    Id = 4,
                    اسم_الاجازة = "إجازة عمرة",
                    مخصومة_من_الرصيد = false,
                    تحتاج_نموذج = true,
                    مفعلة = true
                },
                new LeaveTypes
                {
                    Id = 5,
                    اسم_الاجازة = "إجازة وضع",
                    مخصومة_من_الرصيد = false,
                    تحتاج_نموذج = false,
                    مفعلة = true
                }
            );

            modelBuilder.Entity<LeaveRequest>()
    .Property(l => l.Status)
    .HasDefaultValue(LeaveStatus.قيد_الانتظار);
        }



        public DbSet<Employee> Employee { get; set; }
        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<EmploymentStatus> EmploymentStatuses { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }
        public DbSet<WorkLocation> WorkLocations { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<subDepartment> SubDepartments { get; set; }
        public DbSet<Section> Sections { get; set; }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankBranch> BankBranches { get; set; }

        public DbSet<EmployeeFinancialData> EmployeeFinancialDatas { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<LeaveTypes> LeaveTypes{ get; set; }
    }
}
