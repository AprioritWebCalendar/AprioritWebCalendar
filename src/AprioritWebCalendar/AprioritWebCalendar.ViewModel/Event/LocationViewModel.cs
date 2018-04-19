using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AprioritWebCalendar.Infrastructure.Extensions;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class LocationViewModel : IValidatableObject
    {
        public double? Longitude { get; set; }
        public double? Lattitude { get; set; }
        public string Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if (Longitude == null ^ Lattitude == null)
            {
                errors.AddError("Longitude and Lattitude are required.", nameof(Longitude), nameof(Lattitude));
                return errors;
            }

            if (Longitude.Value < -180 || Longitude.Value > 180)
            {
                errors.AddError("Longitude must be from -180 to 180.", nameof(Longitude));
            }

            if (Lattitude.Value < -90 || Lattitude.Value > 90)
            {
                errors.AddError("Lattitude must be from -90 to 90.", nameof(Lattitude));
            }

            if (!string.IsNullOrEmpty(Description) && Description.Length > 256)
            {
                errors.AddError("The description can't me more than 256 symbols.", nameof(Description));
            }

            return errors;
        }
    }
}
