using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AprioritWebCalendar.Infrastructure.Exceptions;

namespace AprioritWebCalendar.Web.Filters
{
    public class ExceptionHandlerAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(NotFoundException))
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
