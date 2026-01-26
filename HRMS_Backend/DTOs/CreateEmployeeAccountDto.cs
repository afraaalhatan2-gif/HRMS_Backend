using HRMS_Backend.Models;

namespace HRMS_Backend.DTOs
{
    public class CreateEmployeeAccountDto
    {
        //  بيانات الحساب
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }   

        //  بيانات الموظف
        public string EmployeeNumber { get; set; }

        

        public string FullName { get; set; }
        public string Phone1 { get; set; }

        public string? Phone2 { get; set; }
        

        public int AnnualLeaveBalance { get; set; } = 30;

        public string MotherName { get; set; }
        public string NationalId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
       
        public DateTime HireDate { get; set; }

        public int MaritalStatusId { get; set; }
        public int JobTitleId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int DepartmentId { get; set; }
        public int WorkLocationId { get; set; }
        public int JobGradeId { get; set; }

    }
}
