using ConstructionProject.Models;
using System.Security.Claims;

namespace ConstructionProject.Helpers
{
    public static class RoleAuthorizationHelper
    {
        public static bool HasRole(ClaimsPrincipal user, params string[] roles)
        {
            if (user == null) return false;
            return roles.Any(role => user.IsInRole(role));
        }

        public static bool HasRole(ClaimsPrincipal user, UserRole role)
        {
            if (user == null) return false;
            return user.IsInRole(role.ToString());
        }

        public static int? GetUserId(ClaimsPrincipal user)
        {
            var claim = user?.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out var userId) ? userId : null;
        }

        public static string? GetUserRole(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static string? GetUserEmail(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static bool CanManageUsers(ClaimsPrincipal user)
        {
            return HasRole(user, UserRole.Admin.ToString());
        }

        public static bool CanViewProject(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.ProjectManager.ToString(),
                UserRole.SiteEngineer.ToString(),
                UserRole.SafetyOfficer.ToString()
            };
            return HasRole(user, allowedRoles);
        }

        public static bool CanManageInventory(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.ProjectManager.ToString(),
                UserRole.SiteEngineer.ToString()
            };
            return HasRole(user, allowedRoles);
        }

        public static bool CanRecordSafetyInspection(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.SafetyOfficer.ToString(),
                UserRole.SiteEngineer.ToString()
            };
            return HasRole(user, allowedRoles);
        }

        public static bool CanManageTasks(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.ProjectManager.ToString(),
                UserRole.SiteEngineer.ToString()
            };
            return HasRole(user, allowedRoles);
        }
    }
}
