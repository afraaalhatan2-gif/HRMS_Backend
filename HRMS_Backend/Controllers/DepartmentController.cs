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
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var departments = _context.Departments
        .Include(d => d.ManagerEmployee)
        .Include(d => d.Employees)
        .Select(d => new
        {
            d.Id,
            d.Name,
            ManagerEmployeeId = d.ManagerEmployeeId,
            ManagerName = d.ManagerEmployee != null
                ? d.ManagerEmployee.FullName
                : null
        })
        .ToList();

            return Ok(departments);
        }

        [HttpPost]
        public IActionResult AddDepartmentData(CreateDepartmentDto dto)
        {
            var department = new Department
            {
                Name = dto.Name
                // ManagerEmployeeId = null تلقائي
            };

            _context.Departments.Add(department);
            _context.SaveChanges();

            return Ok("تم إنشاء الإدارة بنجاح");
        
    }
    }
}
