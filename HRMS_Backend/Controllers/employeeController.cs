using HRMS_Backend.Attributes;
using HRMS_Backend.Data;
using HRMS_Backend.DTOs;
using HRMS_Backend.DTOs;
using HRMS_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HasPermission("AddEmployee")]
        [HttpPost("create-account")]
        
        public IActionResult CreateEmployeeWithAccount(CreateEmployeeAccountDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                return BadRequest("اسم المستخدم مطلوب");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                return BadRequest("كلمة المرور يجب أن تكون 6 أحرف على الأقل");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return BadRequest("اسم الموظف الكامل مطلوب");

            if (string.IsNullOrWhiteSpace(dto.EmployeeNumber))
                return BadRequest("رقم الموظف مطلوب");

            // التحقق من اسم المستخدم مسبقًا
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest("اسم المستخدم موجود مسبقاً");
            // 1️⃣ تحقق من اليوزر نيم
            if (_context.Users.Any(u => u.Username == dto.Username))
                return BadRequest("اسم المستخدم موجود مسبقاً");

            var role = _context.Roles
       .Include(r => r.RolePermissions)
       .ThenInclude(rp => rp.Permission)
       .FirstOrDefault(r => r.Id == dto.RoleId);

            if (role == null)
                return BadRequest("Role not found");

            //  إنشاء User
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                RoleId = dto.RoleId,
              
            };

            var department = _context.Departments
      .FirstOrDefault(d => d.Id == dto.DepartmentId);

            if (department == null)
                return BadRequest("الإدارة غير موجودة");
            _context.Users.Add(user);
            _context.SaveChanges(); 

            // إنشاء Employee وربطه باليوزر
            var employee = new Employee
            {
                EmployeeNumber = dto.EmployeeNumber,
                FullName = dto.FullName,
                Phone1 = dto.Phone1,
                Phone2 = dto.Phone2,
                MotherName = dto.MotherName,
                NationalId = dto.NationalId,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
               
                HireDate = dto.HireDate,

                MaritalStatusId = dto.MaritalStatusId,
                JobTitleId = dto.JobTitleId,
                EmploymentStatusId = dto.EmploymentStatusId,
                DepartmentId = dto.DepartmentId,
                WorkLocationId = dto.WorkLocationId,
                JobGradeId = dto.JobGradeId,

               
                AnnualLeaveBalance = dto.AnnualLeaveBalance,

                UserId = user.Id,
              
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return Ok("تم إنشاء الموظف والحساب بنجاح");
        }
        [HttpPost("assign-manager")]
        public IActionResult AssignManager(int departmentId, int employeeId)
        {
            var department = _context.Departments
                .FirstOrDefault(d => d.Id == departmentId);

            if (department == null)
                return NotFound("الإدارة غير موجودة");

            var employee = _context.Employees
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                return NotFound("الموظف غير موجود");

            department.ManagerEmployeeId = employeeId;

            _context.SaveChanges();
            return Ok("تم تعيين المدير بنجاح");
        }

        [Authorize]
        [HttpPost("my/education")]
        public IActionResult AddMyEducation(EmployeeEducation dto)
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);

            var employee = _context.Employees
                .FirstOrDefault(e => e.UserId == userId);

            if (employee == null)
                return NotFound();

            dto.EmployeeId = employee.Id;

            _context.EmployeeEducations.Add(dto);
            _context.SaveChanges();

            return Ok("تمت إضافة المؤهل");
        }


        //[HttpPost("create")]
        //        public IActionResult Create([FromBody] CreateEmployeeDto dto)
        //      {
        //        var user = _context.Users.Find(dto.UserId);
        //      if (user == null)
        //        return BadRequest("User not found");
        //
        //          var employee = new Employee
        //        {
        //          EmployeeNumber = dto.EmployeeNumber,
        //        FullName = dto.FullName,
        //      UserId = dto.UserId,
        //    ManagerId = dto.ManagerId,
        //  AnnualLeaveBalance = dto.AnnualLeaveBalance,
        //MotherName = dto.MotherName,
        //                NationalId = dto.NationalId,
        //              BirthDate = dto.BirthDate,
        //              Gender = dto.Gender,
        //             Nationality = dto.Nationality,
        //            HireDate = dto.HireDate,

        //          MaritalStatusId = dto.MaritalStatusId,
        //        JobTitleId = dto.JobTitleId,
        //      EmploymentStatusId = dto.EmploymentStatusId,
        //    DepartmentId = dto.DepartmentId,
        //  WorkLocationId = dto.WorkLocationId,
        //JobGradeId = dto.JobGradeId
        //};

        //_context.Employees.Add(employee);
        //  _context.SaveChanges();

        //return Ok(employee);
        //}

        // Get All
        [HasPermission("ViewEmployee")]
        [HttpGet("all")]
        public IActionResult GetAllEmployees()
        {
            var employees = _context.Employees
       .Include(e => e.MaritalStatus)
       .Include(e => e.JobTitle)
       .Include(e => e.EmploymentStatus)
       .Include(e => e.Department)
       .Include(e => e.WorkLocation)
       .Include(e => e.JobGrade)
       .Select(e => new EmployeeDto
       {
           Id = e.Id,
           FullName = e.FullName,
           MaritalStatus = e.MaritalStatus.Name,
           JobTitle = e.JobTitle.Name,
           EmploymentStatus = e.EmploymentStatus.Name,
           Department = e.Department.Name,
           WorkLocation = e.WorkLocation.Name,
           JobGrade = e.JobGrade.Name
       })
       .ToList();

            return Ok(employees);
        }

        [HasPermission("ViewEmployee")]
        [HttpGet("managers")]
        public IActionResult GetManagers()
        {
            var managers = _context.Employees
                .Include(e => e.User)
                .Where(e => e.User.RoleId == 4)
                .Select(e => new
                {
                    e.Id,
                    e.FullName
                })
                .ToList();

            return Ok(managers);
        }

        // Get By ID
        [HasPermission("ViewEmployee")]
        [HttpGet("{id}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        // Edit
        [HasPermission("EditEmployee")]
        [HttpPut("edit/{id}")]
        public IActionResult EditEmployee(int id, Employee updated)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound("Employee not found");

            // هذا السطر ينسخ كل الحقول من updated إلى employee
            _context.Entry(employee).CurrentValues.SetValues(updated);

            // نضمن إن الـ Id ما يتغيرش
            employee.Id = id;

            _context.SaveChanges();
            return Ok("Employee updated successfully");
        }

        // Delete
        [HasPermission("DeleteEmployee")]
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
                return NotFound("Employee not found");

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return Ok("Employee deleted successfully");
        }
        [HttpGet("my-profile")]
       
        public IActionResult MyProfile()
        {
            var employeeId = User.FindFirst("EmployeeId")?.Value;

            if (string.IsNullOrEmpty(employeeId))
                return Unauthorized("EmployeeId غير موجود في التوكن");

            var employee = _context.Employees
                .Include(e => e.JobTitle)
                .Include(e => e.JobGrade)
                .FirstOrDefault(e => e.Id == int.Parse(employeeId));

            if (employee == null)
                return NotFound("الموظف غير موجود");

            return Ok(new
            {
                id = employee.Id,
                fullName = employee.FullName,
                jobTitle = employee.JobTitle?.Name,
                jobGrade = employee.JobGrade?.Name,
                profileImage = (string?)null
            });
        }
    }
}
