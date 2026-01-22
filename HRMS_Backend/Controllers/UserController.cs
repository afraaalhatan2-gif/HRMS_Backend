using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using HRMS_Backend.Data;
using HRMS_Backend.Data;
using HRMS_Backend.DTOs;
using HRMS_Backend.Models;
using HRMS_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HRMS_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------
        //                Register
        // -----------------------------------------
        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = HashPassword(dto.Password),
                RoleId = dto.RoleId   
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { user.Id, user.Username });
        }

        // -----------------------------------------
        //                Login
        // -----------------------------------------

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var hashed = HashPassword(request.Password);

            var user = _context.Users
        .Include(u => u.Role)
        .FirstOrDefault(u =>
            u.Username == request.Username &&
            u.PasswordHash == hashed);

            if (user == null)
                return Unauthorized("Invalid username or password");

            // نجيب الموظف المرتبط باليوزر
            var employee = _context.Employees
                .Include(e => e.JobTitle)
                .Include(e => e.JobGrade)
                .FirstOrDefault(e => e.UserId == user.Id);

           
            

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token,
                roleId = user.RoleId,
                roleName = user.Role.RoleName
            });
        }



        // -----------------------------------------
        //     Helper: Password Hashing
        // -----------------------------------------
        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // -----------------------------------------
        //     Helper: Generate JWT Token
        // -----------------------------------------
        private string GenerateJwtToken(User user)
        {

            // 1) جلب اسم الدور من الـ user (نظراً لأن Users جدولك يخزن role كـ نص)
            var roleId = user.RoleId;

           
            // 3) جلب أسماء الصلاحيات المرتبطة بهذا الدور
            var userPermissions =
 (
     from rp in _context.RolePermissions
     join p in _context.Permissions on rp.PermissionId equals p.Id
     where rp.RoleId == roleId
     select p.PermissionName
 ).ToList();
            // /3.5) جلب بيانات الموظف المرتبط باليوزر
            var employee = _context.Employees
    .Include(e => e.JobTitle)
    .Include(e => e.JobGrade)
    .FirstOrDefault(e => e.UserId == user.Id);

            var claims = new List<Claim>
{
    // بيانات اليوزر
     // بيانات اليوزر
        new Claim(ClaimTypes.Name, user.Username ?? ""),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim("UserId", user.Id.ToString()),

        // الدور (مهم جداً)
        new Claim(ClaimTypes.Role, user.Role.RoleName), // 👈 هذا كان ناقص
        new Claim("RoleId", roleId.ToString()),

        // بيانات الموظف
        new Claim("EmployeeId", employee?.Id.ToString() ?? ""),
        new Claim("FullName", employee?.FullName ?? ""),
        new Claim("JobTitle", employee?.JobTitle?.Name ?? ""),
        new Claim("JobGrade", employee?.JobGrade?.Name ?? "")
    
        };

            foreach (var perm in userPermissions)
            {
                // نوع الكلايم نستخدمه "permission" (نفس اللي استخدمتِه في HasPermissionAttribute)
                claims.Add(new Claim("permission", perm));
            }

            // 5) المفتاح لازم يكون نفس المفتاح في Program.cs
            var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes("SuperSecretKeyForJWTAuthentication1234567890")
  );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );



            return new JwtSecurityTokenHandler().WriteToken(token);
         
        }

        [Authorize]

        [HttpGet("my-permissions")]
        public IActionResult GetMyPermissions()
        {
            // 1) نجيب اسم اليوزر من التوكن
            var username = User.Identity.Name;

            // 2) نلقّط اليوزر من الداتابيز
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null) return Unauthorized();

            // 3) نجيبو صلاحيات الدور متاعه
            var permissions = (from rp in _context.RolePermissions
                               where rp.RoleId == user.RoleId
                               join p in _context.Permissions on rp.PermissionId equals p.Id
                               select p.PermissionName).ToList();

            // 4) نرجعوهم في JSON
            return Ok(new
            {
                username = user.Username,
                role = user.Role,
                permissions = permissions
            });
        }
    }



    // -----------------------------------------
    //          Login Request Model
    // -----------------------------------------
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
