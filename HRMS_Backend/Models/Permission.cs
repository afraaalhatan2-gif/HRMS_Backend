namespace HRMS_Backend.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }

        public List<RolePermission> RolePermissions { get; set; }

    }
}
