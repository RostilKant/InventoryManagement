using System;
using System.Security.Claims;

namespace Services.ServiceExtensions
{
    public static class CommonExtensions
    {
        public static Guid? GetCurrentUserId(this ClaimsPrincipal claims)
        {
            var userIdString = claims.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdString, out var userId);
            return userId;
        }
    }
}