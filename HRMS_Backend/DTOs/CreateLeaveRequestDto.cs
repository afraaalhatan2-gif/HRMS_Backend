namespace HRMS_Backend.DTOs
{
    public class CreateLeaveRequestDto

    {
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public int TotalDays { get; set; }

        public string? Notes { get; set; }
    }
}
