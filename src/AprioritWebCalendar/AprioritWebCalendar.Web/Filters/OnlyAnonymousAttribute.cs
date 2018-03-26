using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AprioritWebCalendar.Web.Filters
{
    /// <summary>
    /// The filter to close access for registered users
    /// to methods like "Login", "Register".
    /// </summary>
    public class OnlyAnonymousAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // TODO: Fix.
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
