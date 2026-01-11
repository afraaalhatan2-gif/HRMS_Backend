namespace HRMS_Backend.Models
{
    public class LeaveType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
