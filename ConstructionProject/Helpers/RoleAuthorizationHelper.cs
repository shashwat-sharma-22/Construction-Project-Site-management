using ConstructionProject.Models;
using System.Security.Claims;

namespace ConstructionProject.Helpers
{
    /// <summary>
    /// Helper class for role-based authorization checks
    /// </summary>
    public static class RoleAuthorizationHelper
    {
        /// <summary>
        /// Checks if the current user has one of the required roles
        /// </summary>
        public static bool HasRole(ClaimsPrincipal user, params string[] roles)
        {
            if (user == null) return false;
            return roles.Any(role => user.IsInRole(role));
        }

        /// <summary>
        /// Checks if the current user has the specified role
        /// </summary>
        public static bool HasRole(ClaimsPrincipal user, UserRole role)
        {
            if (user == null) return false;
            return user.IsInRole(role.ToString());
        }

        /// <summary>
        /// Gets the user ID from claims
        /// </summary>
        public static int? GetUserId(ClaimsPrincipal user)
        {
            var claim = user?.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out var userId) ? userId : null;
        }

        /// <summary>
        /// Gets the user's role from claims
        /// </summary>
        public static string? GetUserRole(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// Gets the user's email from claims
        /// </summary>
        public static string? GetUserEmail(ClaimsPrincipal user)
        {
            return user?.FindFirst(ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Checks if user can manage other users (Admin only)
        /// </summary>
        public static bool CanManageUsers(ClaimsPrincipal user)
        {
            return HasRole(user, UserRole.Admin.ToString());
        }

        /// <summary>
        /// Checks if user can view project details
        /// </summary>
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

        /// <summary>
        /// Checks if user can manage inventory
        /// </summary>
        public static bool CanManageInventory(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.ProjectManager.ToString(),
                UserRole.SiteEngineer.ToString()
            };
            return HasRole(user, allowedRoles);
        }

        /// <summary>
        /// Checks if user can record safety inspections
        /// </summary>
        public static bool CanRecordSafetyInspection(ClaimsPrincipal user)
        {
            var allowedRoles = new[] { 
                UserRole.Admin.ToString(), 
                UserRole.SafetyOfficer.ToString(),
                UserRole.SiteEngineer.ToString()
            };
            return HasRole(user, allowedRoles);
        }

        /// <summary>
        /// Checks if user can approve/manage tasks
        /// </summary>
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
