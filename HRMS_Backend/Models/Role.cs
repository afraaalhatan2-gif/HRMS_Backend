namespace HRMS_Backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        public List<RolePermission> RolePermissions { get; set; }

    }
}
