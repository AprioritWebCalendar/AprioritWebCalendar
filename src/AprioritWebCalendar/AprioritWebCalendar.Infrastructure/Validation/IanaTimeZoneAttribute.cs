using System.ComponentModel.DataAnnotations;
using AprioritWebCalendar.Infrastructure.DataTypes;

namespace AprioritWebCalendar.Infrastructure.Validation
{
    public class IanaTimeZoneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
                ErrorMessage = "Time Zone name must be IANA TimeZone ID.";

            if (value is string str)
            {
                return TimeZoneInfoIana.IsValidTimeZoneName(str);
            }
            return false;
        }
    }
}
