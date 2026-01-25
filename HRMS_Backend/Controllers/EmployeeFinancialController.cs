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
    public class EmployeeFinancialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeFinancialController(ApplicationDbContext context)
        {
            _context = context;
                }
        [HttpPost]
        [Authorize]
        [HasPermission("AddEmployee")]
        public IActionResult AddFinancialData(CreateEmployeeFinancialDto dto)
        {
            var employee = _context.Employees
     .FirstOrDefault(e => e.Id == dto.EmployeeId);

            if (employee == null)
                return NotFound("الموظف غير موجود");

            var exists = _context.EmployeeFinancialDatas
                .Any(f => f.EmployeeId == dto.EmployeeId);

            if (exists)
                return BadRequest("البيانات المالية موجودة مسبقاً");

            var data = new EmployeeFinancialData
            {
                EmployeeId = dto.EmployeeId,
                BasicSalary = dto.BasicSalary,
                Allowances = dto.Allowances,
                Deductions = dto.Deductions,
                BankId = dto.BankId,
                BankBranchId = dto.BankBranchId
            };

            _context.EmployeeFinancialDatas.Add(data);
            _context.SaveChanges();

            return Ok("تمت إضافة البيانات المالية");
        }

        [HttpGet("my-data")]
        [HasPermission("ViewEmployee")]
        [Authorize]
        public IActionResult GetMyFinancialData()
        {
            var username = User.Identity.Name;

            var employee = _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Username == username);

            if (employee == null)
                return NotFound("الموظف غير موجود");

            var data = _context.EmployeeFinancialDatas
                .Where(f => f.EmployeeId == employee.Id)
                .Select(f => new
                {
                    f.BasicSalary,
                    f.Allowances,
                    f.Deductions,
                    f.BankId,
                    f.BankBranchId,
                })
                .FirstOrDefault();

            if (data == null)
                return NotFound("لا توجد بيانات مالية");

            return Ok(data);
        }
        [HttpGet("get-all")]
        [HasPermission("ViewEmployee")]
        [Authorize]
        public IActionResult GetAllFinancialData()
        {
            var data = _context.EmployeeFinancialDatas
                .Include(f => f.Employee)
                    .ThenInclude(e => e.JobTitle)
                .Include(f => f.Bank)
                .Include(f => f.BankBranch)
                .Select(f => new
                {
                    EmployeeId = f.EmployeeId,
                    FullName = f.Employee.FullName,
                    JobTitle = f.Employee.JobTitle.Name,

                    BasicSalary = f.BasicSalary,
                    Allowances = f.Allowances,
                    Deductions = f.Deductions,

                    BankName = f.Bank.Name,
                    BankBranch = f.BankBranch.Name
                })
                .ToList();

            return Ok(data);
        }



        [HttpPut("{id}")]

        
        public IActionResult UpdateFinancialData(int id, CreateEmployeeFinancialDto dto)
        {
            var data = _context.EmployeeFinancialDatas
                .FirstOrDefault(x => x.Id == id);

            if (data == null)
                return NotFound("البيانات المالية غير موجودة");

            data.BasicSalary = dto.BasicSalary;
            data.Allowances = dto.Allowances;
            data.Deductions = dto.Deductions;
            data.BankId = dto.BankId;
            data.BankBranchId = dto.BankBranchId;

            _context.SaveChanges();
            return Ok("تم تحديث البيانات المالية بنجاح");
        }
        [HttpDelete("{id}")]
        [HasPermission("DeleteFinancialData")]
        public IActionResult DeleteFinancialData(int id)
        {
            var data = _context.EmployeeFinancialDatas
                .FirstOrDefault(x => x.Id == id);

            if (data == null)
                return NotFound("البيانات المالية غير موجودة");

            _context.EmployeeFinancialDatas.Remove(data);
            _context.SaveChanges();

            return Ok("تم حذف البيانات المالية");
        }

    }

}
    
