using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AprioritWebCalendar.Web.Extensions;

namespace AprioritWebCalendar.Web.Filters
{
    public class ValidateApiModelFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action.
        }

        /// <summary>
        /// The methods executes after action executing.
        /// 
        /// If ModelState exists and it's invalid,
        /// action sends bad request with errors as string array.
        /// 
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState?.IsValid == false)
            {
                context.Result = (context.Controller as Controller).BadRequest(context.ModelState.ToStringEnumerable());
            }
        }
    }
}
