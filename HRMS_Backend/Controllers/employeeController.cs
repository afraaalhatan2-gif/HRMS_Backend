using HRMS_Backend.Attributes;
using HRMS_Backend.Data;
using HRMS_Backend.DTOs;
using HRMS_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HRMS_Backend.DTOs;

namespace HRMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HasPermission("AddEmployee")]
        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateEmployeeDto dto)
        {
            var user = _context.Users.Find(dto.UserId);
            if (user == null)
                return BadRequest("User not found");

            var employee = new Employee
            {
                EmployeeNumber = dto.EmployeeNumber,
                FullName = dto.FullName,
                UserId = dto.UserId,
                ManagerId = dto.ManagerId,
                AnnualLeaveBalance = dto.AnnualLeaveBalance,
                MotherName = dto.MotherName,
                NationalId = dto.NationalId,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Nationality = dto.Nationality,
                HireDate = dto.HireDate,

                MaritalStatusId = dto.MaritalStatusId,
                JobTitleId = dto.JobTitleId,
                EmploymentStatusId = dto.EmploymentStatusId,
                DepartmentId = dto.DepartmentId,
                WorkLocationId = dto.WorkLocationId,
                JobGradeId = dto.JobGradeId
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return Ok(employee);
        }

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
    }
}
