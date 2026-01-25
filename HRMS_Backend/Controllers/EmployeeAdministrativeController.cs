using System;
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
    public class EmployeeAdministrativeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeAdministrativeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [HasPermission("AddEmployee")]
        public IActionResult Add(CreateEmployeeAdministrativeDto dto)
        {
            // نتأكد الموظف موجود
            var employee = _context.Employees
       .FirstOrDefault(e => e.Id == dto.EmployeeId);

            if (employee == null)
                return NotFound("الموظف غير موجود");

            var data = new EmployeeAdministrativeData
            {
                EmployeeId = dto.EmployeeId,
                ContractType = dto.ContractType,
                FileNumber = dto.FileNumber,
                ContractStartDate = dto.ContractStartDate,
                ContractEndDate = dto.ContractEndDate
            };

            _context.EmployeeAdministrativeDatas.Add(data);
            _context.SaveChanges();

            return Ok("تمت إضافة البيانات الإدارية بنجاح");
        }

        [HttpGet("my-data")]
        [HasPermission("ViewEmployee")]
        [Authorize]
        public IActionResult GetMyAdministrativeData()
        {
            var username = User.Identity.Name;

            var employee = _context.Employees
                .Include(e => e.User)
                .FirstOrDefault(e => e.User.Username == username);

            if (employee == null)
                return NotFound("الموظف غير موجود");

            var adminData = _context.EmployeeAdministrativeDatas
                .Where(a => a.EmployeeId == employee.Id)
                .Select(a => new
                {
                    a.Id,
                    a.ContractType,
                    a.FileNumber,
                    a.ContractStartDate,
                    a.ContractEndDate
                })
                .FirstOrDefault();

            if (adminData == null)
                return NotFound("لا توجد بيانات إدارية");

            return Ok(adminData);
        }

       

            [HttpGet("get-all")]
        [HasPermission("ViewEmployee")]

        public IActionResult GetAllAdministrativeData()
            {
                var data = _context.EmployeeAdministrativeDatas
                    .Include(a => a.Employee)
                        .ThenInclude(e => e.JobTitle)
                    .Include(a => a.Employee)
                        .ThenInclude(e => e.JobGrade)
                    .Select(a => new
                    {
                        EmployeeId = a.EmployeeId,
                        FullName = a.Employee.FullName,
                        JobTitle = a.Employee.JobTitle.Name,
                        JobGrade = a.Employee.JobGrade.Name,

                        ContractType = a.ContractType,
                        FileNumber = a.FileNumber,
                        ContractStartDate = a.ContractStartDate,
                        ContractEndDate = a.ContractEndDate
                    })
                    .ToList();

                return Ok(data);
            }
        [HttpPut("{id}")]
       
        public IActionResult UpdateAdministrativeData(int id, EmployeeAdministrativeData dto)
        {
            var data = _context.EmployeeAdministrativeDatas
                .FirstOrDefault(x => x.Id == id);

            if (data == null)
                return NotFound("البيانات الإدارية غير موجودة");

            data.ContractType = dto.ContractType;
            data.FileNumber = dto.FileNumber;
            data.ContractStartDate = dto.ContractStartDate;
            data.ContractEndDate = dto.ContractEndDate;

            _context.SaveChanges();
            return Ok("تم تحديث البيانات الإدارية بنجاح");
        }
        [HttpDelete("{id}")]
       
        public IActionResult DeleteAdministrativeData(int id)
        {
            var data = _context.EmployeeAdministrativeDatas
                .FirstOrDefault(x => x.Id == id);

            if (data == null)
                return NotFound("البيانات الإدارية غير موجودة");

            _context.EmployeeAdministrativeDatas.Remove(data);
            _context.SaveChanges();

            return Ok("تم حذف البيانات الإدارية");
        }
    }
    }

