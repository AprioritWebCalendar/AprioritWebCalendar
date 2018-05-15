using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AprioritWebCalendar.Web.Filters
{
    public class ValidateTelegramUpdateAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState?.IsValid != true)
            {
                context.Result = new OkResult();
                return;
            }
        }
    }
}
