using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using HRMS_Backend.Data;


namespace HRMS_Backend.Attributes
{
    public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permission;

        public HasPermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (role == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var db = (ApplicationDbContext)context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext));

            var roleId = db.Roles.FirstOrDefault(r => r.RoleName == role)?.Id;

            if (roleId == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // هل الدور يملك الصلاحية المطلوبة؟
            var permissionExists = db.RolePermissions
                .Any(rp => rp.RoleId == roleId && rp.Permission.PermissionName == _permission);

            if (!permissionExists)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
