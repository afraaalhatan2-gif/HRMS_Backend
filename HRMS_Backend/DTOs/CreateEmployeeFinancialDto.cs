namespace HRMS_Backend.DTOs
{
    public class CreateEmployeeFinancialDto
    {
        public int EmployeeId { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Deductions { get; set; }

        public int BankId { get; set; }
        public int BankBranchId { get; set; }
        
    }
}
