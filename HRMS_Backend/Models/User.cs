using System;

namespace HRMS_Backend.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }  // مش بنخزن الباسورد نفسه

      
        public int RoleId { get; set; }
        public Role Role { get; set; }  // SuperAdmin, Manager, Employee ...

        public Employee? Employee { get; set; }
    
}
}
