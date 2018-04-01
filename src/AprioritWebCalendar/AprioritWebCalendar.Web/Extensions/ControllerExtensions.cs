using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AprioritWebCalendar.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static int GetUserId(this Controller controller)
        {
            string strId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = int.TryParse(strId, out int id);

            if (!success)
                throw new InvalidOperationException();

            return id;
        }

        public static IActionResult OkOrNotFound<T>(this Controller controller, T obj)
        {
            if (obj == null)
                return controller.NotFound();

            return controller.Ok(obj);
        }

        public static IActionResult OkOrNoContent<T>(this Controller controller, IEnumerable<T> items)
        {
            if (items == null || items?.Any() == false)
                return controller.NoContent();

            return controller.Ok(items);
        }

        /// <summary>
        /// API always sends errors in string array.
        /// The method helps to don't write much code.
        /// </summary>
        public static IActionResult BadRequestError(this Controller controller, string error)
        {
            return controller.BadRequest(new[] { error });
        }
    }
}
