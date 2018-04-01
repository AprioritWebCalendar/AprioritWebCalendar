using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AprioritWebCalendar.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {
        public static void AddError(this IList<ValidationResult> list, string error)
        {
            list.Add(new ValidationResult(error));
        }

        public static void AddError(this IList<ValidationResult> list, string error, params string[] members)
        {
            list.Add(new ValidationResult(error, members));
        }
    }
}
