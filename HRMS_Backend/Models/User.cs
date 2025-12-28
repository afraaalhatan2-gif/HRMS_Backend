using System;

namespace HRMS_Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }  // مش بنخزن الباسورد نفسه

        public string Role { get; set; }  // SuperAdmin, Manager, Employee ...
        public int RoleId { get; set; }
    }
}
