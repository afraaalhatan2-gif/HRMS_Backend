namespace HRMS_Backend.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string MaritalStatus { get; set; }
        public string JobTitle { get; set; }
        public string EmploymentStatus { get; set; }
        public string Department { get; set; }
        public string WorkLocation { get; set; }
        public string JobGrade { get; set; }
    }
}
