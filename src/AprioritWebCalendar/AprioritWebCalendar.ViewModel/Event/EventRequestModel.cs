using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AprioritWebCalendar.Infrastructure.Extensions;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventRequestModel : IValidatableObject
    {
        [BindNever]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int CalendarId { get; set; }

        [DataType(DataType.Date)]
        public DateTimeOffset? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTimeOffset? EndDate { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? StartTime { get; set; }
        [DataType(DataType.Time)]
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }
        public int? RemindBefore { get; set; }
        public bool IsPrivate { get; set; } = true;

        public LocationViewModel Location { get; set; }
        public PeriodViewModel Period { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            StartDate = StartDate?.Date;
            EndDate = EndDate?.Date;

            var errors = new List<ValidationResult>();

            if (Name?.Length < 3 || Name?.Length > 32)
            {
                errors.AddError("The name field is required and must be a string from 3 to 32 symbols.", nameof(Name));
            }

            if (RemindBefore != null && RemindBefore < 1)
            {
                errors.AddError("Remind Before must be more than 0.", nameof(RemindBefore));
            }

            if (Location != null)
            {
                errors.AddRange(Location.Validate(validationContext));
            }

            if (!string.IsNullOrEmpty(Description) && Description.Length > 256)
            {
                errors.AddError("The description field can't be more than 256 symbols.", nameof(Description));
            }

            if (IsAllDay)
            {
                StartTime = null;
                EndTime = null;
            }
            else
            {
                if (StartTime > EndTime)
                {
                    errors.AddError("StartTime must not be more than EndTime.", nameof(StartTime), nameof(EndTime));
                }
            }

            if (Period != null)
            {
                errors.AddRange(Period.Validate(validationContext));
                StartDate = null;
                EndDate = null;
            }
            else
            {
                if (StartDate > EndDate)
                {
                    errors.AddError("StartDate must not be more than EndDate.", nameof(StartDate), nameof(EndDate));
                }
            }

            return errors;
        }
    }
}
