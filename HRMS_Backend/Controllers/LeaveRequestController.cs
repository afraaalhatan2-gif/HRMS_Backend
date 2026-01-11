using HRMS_Backend.Attributes;
using HRMS_Backend.Data;
using HRMS_Backend.DTOs;
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
            // نتحقق من الموظف
            var employee = _context.Employees
                .FirstOrDefault(e => e.Id == dto.EmployeeId);

            if (employee == null)
                return BadRequest("Employee not found");

            // نتحقق من نوع الإجازة
            var leaveType = _context.LeaveTypes
                .FirstOrDefault(l => l.Id == dto.LeaveTypeId);

            if (leaveType == null)
                return BadRequest("Leave type not found");

            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                LeaveTypeId = dto.LeaveTypeId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                TotalDays = (dto.ToDate - dto.FromDate).Days + 1,
                Notes = dto.Notes,
                Status = "Pending"
            };

            _context.LeaveRequests.Add(leave);
            _context.SaveChanges();

            return Ok("Leave request created successfully");
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
                .Select(l => new
                {
                    l.Id,
                    Employee = l.Employee.FullName,
                    LeaveType = l.LeaveType.Name,
                    l.FromDate,
                    l.ToDate,
                    l.TotalDays,
                    l.Notes,
                    l.Status
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
            var leave = _context.LeaveRequests.Find(id);
            if (leave == null) return NotFound();

            leave.Status = "Approved";
            _context.SaveChanges();

            return Ok("Leave approved");
        }

        // Reject Leave

        [Authorize]
        [HasPermission("ApproveLeave")]
        [HttpPut("reject/{id}")]
        public IActionResult Reject(int id)
        {
            var leave = _context.LeaveRequests.Find(id);
            if (leave == null) return NotFound();

            leave.Status = "Rejected";
            _context.SaveChanges();

            return Ok("Leave rejected");
        }
    }
}
