using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AprioritWebCalendar.Infrastructure.Enums;
using AprioritWebCalendar.Infrastructure.Extensions;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class PeriodViewModel : IValidatableObject
    {
        public PeriodType Type { get; set; }

        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public int? Cycle { get; set; }

        [BindNever]
        public int WholeDaysCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Type != PeriodType.Custom)
                Cycle = null;

            var errors = new List<ValidationResult>();

            if (PeriodStart >= PeriodEnd)
            {
                errors.AddError("PeriodStart must not be more than PeriodEnd.", nameof(PeriodStart), nameof(PeriodEnd));
            }

            return errors;
        }
    }
}
