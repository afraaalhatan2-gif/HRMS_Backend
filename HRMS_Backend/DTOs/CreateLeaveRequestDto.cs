using System.ComponentModel.DataAnnotations;
namespace HRMS_Backend.DTOs
{
    public class CreateLeaveRequestDto

    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "نوع الإجازة مطلوب")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        public DateTime ToDate { get; set; }

        [Range(1, 365, ErrorMessage = "عدد الأيام لازم يكون أكبر من صفر")]
        public int TotalDays { get; set; }

        [MaxLength(500, ErrorMessage = "الملاحظات طويلة هلبا")]
        public string? Notes { get; set; }
    }
}
