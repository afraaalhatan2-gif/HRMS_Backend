using System.Security.Claims;
using HRMS_Backend.Attributes;
using HRMS_Backend.Data;
using HRMS_Backend.DTOs;
using HRMS_Backend.Enums;
using HRMS_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Leave Request
        [Authorize]
        [HasPermission("SubmitLeave")]
        [HttpPost("create")]
        public IActionResult Create(CreateLeaveRequestDto dto)
        {
           
    if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //  فاليديشن منطقي للتواريخ
            if (dto.ToDate < dto.FromDate)
                return BadRequest("تاريخ النهاية ما ينفعش يكون قبل البداية");

            // 🔹 نجيب اليوزر الحالي
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return Unauthorized("User not found in token");

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "SuperAdmin")
                return BadRequest("السوبر أدمن ما يقدرش يقدّم إجازة");

            // 🔹 نجيب الموظف المرتبط باليوزر
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var employee = _context.Employee
                .FirstOrDefault(e => e.UserId == userId);

            if (employee == null)
                return BadRequest("الموظف غير موجود في النظام");

            // 🔹 نتحقق من نوع الإجازة
            var leaveType = _context.LeaveTypes
                .FirstOrDefault(l => l.Id == dto.LeaveTypeId);

            if (leaveType == null)
                return BadRequest("نوع الإجازة غير موجود");

            var totalDays = (dto.ToDate.Date - dto.FromDate.Date).Days + 1;

            var leave = new LeaveRequest
            {
                EmployeeId = employee.Id,
                LeaveTypeId = dto.LeaveTypeId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                TotalDays = totalDays,
                Notes = dto.Notes,
                Status = LeaveStatus.قيد_الانتظار
            };

            _context.LeaveRequests.Add(leave);
            _context.SaveChanges();

            return Ok("تم إرسال طلب الإجازة بنجاح");
        }

        // Get All Leave Requests
        [Authorize]
        [HasPermission("ApproveLeave")]
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var data = _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Select(l => new LeaveRequestResponseDto
                {
                    Id = l.Id,
                    EmployeeName = l.Employee.FullName,
                    LeaveType = l.LeaveType.اسم_الاجازة,
                    FromDate = l.FromDate,
                    ToDate = l.ToDate,
                    TotalDays = l.TotalDays,

                    Status = l.Status.ToString(), 
                    RejectionReason = l.Status == LeaveStatus.مرفوض
                                ? l.ManagerNote
                                : null
                })
                .ToList();

            return Ok(data);
        }

        // Approve Leave
        [Authorize]
        [HasPermission("ApproveLeave")]
        [HttpPut("approve/{id}")]
        public IActionResult Approve(int id)
        {
            var leave = _context.LeaveRequests.FirstOrDefault(l => l.Id == id);

            if (leave == null)
                return NotFound("طلب الإجازة غير موجود");

            leave.Status = LeaveStatus.موافق_المدير;

            _context.SaveChanges();

            return Ok("تمت الموافقة على الإجازة");
        }

        // Reject Leave

        [Authorize]
        [HasPermission("ApproveLeave")]
        [HttpPut("reject/{id}")]
        public IActionResult RejectLeave(int id, [FromBody] string reason)
        {
            var leave = _context.LeaveRequests.FirstOrDefault(l => l.Id == id);

            if (leave == null)
                return NotFound("طلب الإجازة غير موجود");

            leave.Status = LeaveStatus.مرفوض;
            leave.ManagerNote = reason;

            _context.SaveChanges();

            return Ok("تم رفض الإجازة");
        }
    }
}
