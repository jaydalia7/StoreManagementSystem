using System.Security.Claims;

namespace StoreManagementSystem.Extensions
{
    public static class ClaimExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? int.Parse(userId) : 0;
        }
        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            var userRole = principal.FindFirst(ClaimTypes.Role)?.Value;
            return userRole != null ? userRole : string.Empty;
        }
    }
}
