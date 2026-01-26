namespace HRMS_Backend.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // مدير الإدارة (موظف)
        public int? ManagerEmployeeId { get; set; }
        public Employee? ManagerEmployee { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
