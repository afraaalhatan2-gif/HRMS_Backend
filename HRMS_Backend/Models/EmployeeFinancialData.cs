namespace HRMS_Backend.Models
{
    public class EmployeeFinancialData
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public decimal BasicSalary { get; set; }
        public decimal? Allowances { get; set; }
        public decimal? Deductions { get; set; }


        public int BankId { get; set; }

        public Bank Bank { get; set; }

        public int BankBranchId { get; set; }

        public BankBranch BankBranch { get; set; }
    }
}
