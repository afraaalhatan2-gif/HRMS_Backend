using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Text;
using HRMS_Backend.Data;
using HRMS_Backend.Data;
using HRMS_Backend.Models;
using HRMS_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register(User user)
        {
            // تشفير الباسورد
            user.PasswordHash = HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User Registered Successfully!");
        }

        // -----------------------------------------
        //                Login
        // -----------------------------------------

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var hashed = HashPassword(request.Password);
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username && u.PasswordHash == hashed);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var token = GenerateJwtToken(user);

            return Ok(new { token, role = user.Role });
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
            var roleName = user.Role;

            // 2) جلب roleId من جدول Roles (لو موجود)
            var role = _context.Roles.FirstOrDefault(r => r.RoleName == roleName);
            var roleId = role?.Id ?? 0; // لو ما لقيش الدور يساوي 0

            // 3) جلب أسماء الصلاحيات المرتبطة بهذا الدور
            var userPermissions = new List<string>();
            if (roleId != 0)
            {
                userPermissions = (from rp in _context.RolePermissions
                                   where rp.RoleId == roleId
                                   join p in _context.Permissions on rp.PermissionId equals p.Id
                                   select p.PermissionName).ToList();
            }

            // 4) بناء الـ claims (اسم، دور، وصلاحيات منفصلة)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username ?? ""),
        new Claim(ClaimTypes.Role, roleName ?? "")
    };

            foreach (var perm in userPermissions)
            {
                // نوع الكلايم نستخدمه "permission" (نفس اللي استخدمتِه في HasPermissionAttribute)
                claims.Add(new Claim("permission", perm));
            }

            // 5) المفتاح لازم يكون نفس المفتاح في Program.cs
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SuperSecretKeyForJWTAuthentication1234567890")); // غيّريه لو عندِك مفتاح آخر
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            foreach (var perm in userPermissions)
            {
                Console.WriteLine("PERMISSION: " + perm);
                claims.Add(new Claim("permission", perm));
            }
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
