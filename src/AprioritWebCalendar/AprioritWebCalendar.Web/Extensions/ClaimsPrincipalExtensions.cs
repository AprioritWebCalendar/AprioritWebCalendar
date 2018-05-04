using System.Collections.Generic;
using System.Security.Claims;

namespace AprioritWebCalendar.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal claims)
        {
            string strId = claims.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = int.TryParse(strId, out int id);

            if (!success)
                throw new KeyNotFoundException();

            return id;
        }
    }
}
