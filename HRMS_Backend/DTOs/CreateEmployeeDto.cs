namespace HRMS_Backend.DTOs
{
    public class CreateEmployeeDto
    {
        public string EmployeeNumber { get; set; }
        public string FullName { get; set; }
        public string MotherName { get; set; }
        public string NationalId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime HireDate { get; set; }

        public int MaritalStatusId { get; set; }
        public int JobTitleId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int DepartmentId { get; set; }
        public int WorkLocationId { get; set; }
        public int JobGradeId { get; set; }
    }
}
