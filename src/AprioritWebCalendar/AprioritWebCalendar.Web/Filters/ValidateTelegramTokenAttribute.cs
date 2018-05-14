using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using AprioritWebCalendar.Infrastructure.Options;

namespace AprioritWebCalendar.Web.Filters
{
    public class ValidateTelegramTokenAttribute : Attribute, IActionFilter
    {
        private readonly TelegramOptions _telegramOptions;

        public ValidateTelegramTokenAttribute(IOptions<TelegramOptions> options)
        {
            _telegramOptions = options.Value;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenExists = context.ActionArguments.TryGetValue("token", out object tokenObj);

            if (!tokenExists || !tokenObj.ToString().Equals(_telegramOptions.BotToken))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
