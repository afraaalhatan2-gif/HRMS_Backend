namespace HRMS_Backend.Models
{
    public class Employee
    {
        public int Id { get; set; }
       
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string Phone1 { get; set; }

        public string? Phone2 { get; set; }
        public string MotherName { get; set; }
        public string NationalId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime HireDate { get; set; }

        public int MaritalStatusId { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int JobTitleId { get; set; }
        public JobTitle JobTitle { get; set; }
        public int EmploymentStatusId { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int WorkLocationId { get; set; }
        public WorkLocation WorkLocation { get; set; }
        public int JobGradeId { get; set; }
        public JobGrade JobGrade { get; set; }

        public int UserId { get; set; }
        
        public User User { get; set; }

        public int AnnualLeaveBalance { get; set; } = 30;
        public int? ManagerId { get; set; }

        public Employee? Manager { get; set; }

        public ICollection<Employee> Subordinates { get; set; }
       = new List<Employee>();


        public ICollection<EmployeeEducation> Educations { get; set; }

        public EmployeeAdministrativeData AdministrativeData { get; set; }
    }
}
