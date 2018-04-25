using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventsRequestModel : IValidatableObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<int> CalendarsIds { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (!CalendarsIds.Any())
            {
                result.Add(new ValidationResult("You have to choose at least one calendar to get events.", new[] { nameof(CalendarsIds) }));
            }

            if (StartDate > EndDate)
            {
                result.Add(new ValidationResult("StartDate must not be more than EndDate.", new[] { nameof(StartDate), nameof(EndDate) }));
            }

            return result;
        }
    }
}
