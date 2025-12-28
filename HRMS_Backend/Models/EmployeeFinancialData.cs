namespace HRMS_Backend.Models
{
    public class EmployeeFinancialData
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public decimal Salary { get; set; }
        public int BankId { get; set; }

        public int BankBranchId { get; set; }
    }
}
