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

            var holidays = _context.OfficialHolidays
    .Select(h => h.Date.Date)
    .ToList();

            int totalDays = 0;

            for (var date = dto.FromDate.Date; date <= dto.ToDate.Date; date = date.AddDays(1))
            {
                // الجمعة والسبت ما يتحسبوش
                if (date.DayOfWeek == DayOfWeek.Friday ||
                    date.DayOfWeek == DayOfWeek.Saturday)
                    continue;

                // العطل الرسمية ما تتحسبش
                if (holidays.Contains(date))
                    continue;

                totalDays++;
            }
            if (employee.AnnualLeaveBalance < totalDays)
                return BadRequest("رصيد الإجازات غير كافي");

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

            var employees = _context.Employee
                .FirstOrDefault(e => e.UserId == userId);

            // نجيب المدير
            var manager = _context.Employee
                .FirstOrDefault(m => m.Id == employees.ManagerId);

            if (manager != null)
            {
                var notification = new Notification
                {
                    UserId = manager.UserId,
                    Title = "طلب إجازة جديد",
                    Message = $"تم إرسال طلب إجازة من الموظف {employee.FullName}"
                };

                _context.Notifications.Add(notification);
                _context.SaveChanges();
            }

            return Ok("تم إرسال طلب الإجازة بنجاح");



        }

        [Authorize]
        [HasPermission("SubmitLeave")]
        [HttpGet("my-requests")]
        
        public IActionResult MyLeaveRequests()
        {
            var username = User.Identity.Name;

            var employee = _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Username == username);

            if (employee == null)
                return NotFound("الموظف غير موجود");



            var requests = _context.LeaveRequests
               .Include(l => l.LeaveType)
                .Where(l => l.EmployeeId == employee.Id)
                .Select(l => new
                {
                    l.Id,
                    LeaveTypeName = l.LeaveType ,
               
                    l.FromDate,
                    l.ToDate,
                    l.TotalDays,
                    Status = l.Status.ToString(),
                    ManagerNote = l.ManagerNote
                })
                .ToList();

            return Ok(new
            {
                Balance = employee.AnnualLeaveBalance,
                Requests = requests
            });
        }

        // Get All Leave Requests
       // [Authorize]
       // [HasPermission("ApproveLeave")]
       // [HttpGet("all")]
      //  public IActionResult GetAll()
      //  {
         //var data = _context.LeaveRequests
           //     .Include(l => l.Employee)
             //   .Include(l => l.LeaveType)
               // .Select(l => new LeaveRequestResponseDto
               // {
                 //   Id = l.Id,
                   // EmployeeName = l.Employee.FullName,
                    //LeaveType = l.LeaveType.اسم_الاجازة,
                    //FromDate = l.FromDate,
                   // ToDate = l.ToDate,
                  //  TotalDays = l.TotalDays,

                    //Status = l.Status.ToString(), 
                   // RejectionReason = l.Status == LeaveStatus.مرفوض
                     //           ? l.ManagerNote
                       //         : null
                //})
                //.ToList();

            //return Ok(data);
        //}
        [HttpGet("manager/pending")]
        [HasPermission("ApproveLeave")]
        public IActionResult ManagerPendingRequests()
        {
            var username = User.Identity.Name;

            var manager = _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Username == username);

            if (manager == null)
                return NotFound("المدير غير موجود");

            var requests = _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.Employee.ManagerId == manager.Id &&
                            l.Status == LeaveStatus.قيد_الانتظار)
                .Select(l => new
                {
                    l.Id,
                    EmployeeName = l.Employee.FullName,
                    l.FromDate,
                    l.ToDate,
                    l.TotalDays,

                     LeaveTypeId = l.LeaveTypeId,
                    LeaveTypeName = l.LeaveType.اسم_الاجازة,

                    Status = l.Status.ToString(),
                    l.Notes
                })
                .ToList();

            return Ok(requests);
        }


        [HttpPost("{id}/manager-decision")]
        [HasPermission("ApproveLeave")]
        public IActionResult ManagerDecision(int id, bool approve, string? note)
        {
            var leave = _context.LeaveRequests
    .Include(l => l.LeaveType)   
    .Include(l => l.Employee)
        .ThenInclude(e => e.User)
    .FirstOrDefault(l => l.Id == id);

            if (leave == null)
                return NotFound("طلب الإجازة غير موجود");

            if (leave.Status != LeaveStatus.قيد_الانتظار)
                return BadRequest("الطلب تم التعامل معه مسبقاً");

            var employeeUserId = leave.Employee.UserId;
            if (approve)
            {
                // نخصم فقط لو نوع الإجازة مخصومة من الرصيد
                if (leave.LeaveType.مخصومة_من_الرصيد)
                {
                    if (leave.Employee.AnnualLeaveBalance < leave.TotalDays)
                        return BadRequest("رصيد الإجازات غير كافي");

                    leave.Employee.AnnualLeaveBalance -= leave.TotalDays;
                }
                leave.Status = LeaveStatus.موافق_المدير;

                var notification = new Notification
                {
                    UserId = employeeUserId,
                    Title = "تمت الموافقة على طلب الإجازة",
                    Message = $"تمت الموافقة على طلب الإجازة من {leave.FromDate:yyyy-MM-dd} إلى {leave.ToDate:yyyy-MM-dd}",
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);
            }
            else
            {
                leave.Status = LeaveStatus.مرفوض;
;
                leave.ManagerNote = note;
                var notification = new Notification
                {
                    UserId = employeeUserId,
                    Title = "تم رفض طلب الإجازة",
                    Message = $"تم رفض طلب الإجازة. السبب: {note ?? "لم يتم توضيح السبب"}",
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);

            }

            _context.SaveChanges();
            return Ok("تم تحديث حالة الطلب");
        }
        // Approve Leave
       // [Authorize]
        //[HasPermission("ApproveLeave")]
    //    [HttpPut("approve/{id}")]
      //  public IActionResult Approve(int id)
        //{
          //  var leave = _context.LeaveRequests.FirstOrDefault(l => l.Id == id);

            //if (leave == null)
              //  return NotFound("طلب الإجازة غير موجود");

            //leave.Status = LeaveStatus.موافق_المدير;

//            _context.SaveChanges();

  //          return Ok("تمت الموافقة على الإجازة");
    //    }

        // Reject Leave

      //  [Authorize]
        //[HasPermission("ApproveLeave")]
        //[HttpPut("reject/{id}")]
        //public IActionResult RejectLeave(int id, [FromBody] string reason)
        //{
          //  var leave = _context.LeaveRequests.FirstOrDefault(l => l.Id == id);

            //if (leave == null)
              //  return NotFound("طلب الإجازة غير موجود");

            //leave.Status = LeaveStatus.مرفوض;
            //leave.ManagerNote = reason;

            //_context.SaveChanges();

            //return Ok("تم رفض الإجازة");
        //}
    }
}
