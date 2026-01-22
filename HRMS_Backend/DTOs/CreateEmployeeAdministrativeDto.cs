namespace HRMS_Backend.DTOs
{
    public class CreateEmployeeAdministrativeDto
    {
        public int EmployeeId { get; set; }

        public string ContractType { get; set; }
        public string FileNumber { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
    
}
}
