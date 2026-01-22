namespace HRMS_Backend.Models
{
    public class EmployeeAdministrativeData
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string ContractType { get; set; }
        public string FileNumber { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
    }
}
