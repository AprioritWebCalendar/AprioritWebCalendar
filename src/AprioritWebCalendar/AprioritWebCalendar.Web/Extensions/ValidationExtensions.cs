using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AprioritWebCalendar.Web.Extensions
{
    /// <summary>
    /// Just extensions for usebility.
    /// 
    /// API sends errors with HTTP400 as string array.
    /// </summary>
    public static class ValidationExtensions
    {
        public static IEnumerable<string> ToStringEnumerable(this ModelStateDictionary modelState)
        {
            return modelState.Values.Where(e => e.Errors.Count > 0)
                    .SelectMany(e => e.Errors)
                    .Select(e => e.ErrorMessage);
        }

        public static IEnumerable<string> ToStringEnumerable(this IEnumerable<IdentityError> errors)
        {
            var list = new List<string>();

            foreach (var err in errors)
            {
                list.Add(err.Description);
            }
            return list;
        }

        public static ModelStateDictionary ToModelState(this IEnumerable<ValidationResult> errors, ModelStateDictionary modelState = null)
        {
            if (modelState == null)
                modelState = new ModelStateDictionary();

            foreach (var err in errors)
            {
                modelState.AddModelError(err.MemberNames.FirstOrDefault(), err.ErrorMessage);
            }
            return modelState;
        }
    }
}
