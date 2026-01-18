using System.ComponentModel.DataAnnotations;
namespace HRMS_Backend.DTOs
{
    public class CreateLeaveRequestDto

    {
      

        [Required(ErrorMessage = "نوع الإجازة مطلوب")]
        public int LeaveTypeId { get; set; }

        public string LeaveTypeName { get; set; }

        [Required(ErrorMessage = "تاريخ البداية مطلوب")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "تاريخ النهاية مطلوب")]
        public DateTime ToDate { get; set; }

     

        [MaxLength(500, ErrorMessage = "الملاحظات طويلة هلبا")]
        public string? Notes { get; set; }
    }
}
