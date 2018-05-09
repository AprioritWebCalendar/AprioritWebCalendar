using System;
using TimeZoneConverter;

namespace AprioritWebCalendar.Infrastructure.DataTypes
{
    public class TimeZoneInfoIana
    {
        private readonly string _name;

        public TimeZoneInfoIana(string ianaName)
        {
            _name = ianaName;
        }

        public TimeZoneInfoIana(TimeZoneInfo timeZoneInfo)
        {
            _name = TZConvert.WindowsToIana(timeZoneInfo.Id);
        }

        public TimeZoneInfo ToWindowsTimeZoneInfo()
        {
            return TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(_name));
        }

        public DateTime ConvertFromUtc(DateTime dateTimeUtc)
        {
            dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, ToWindowsTimeZoneInfo());
        }

        public static bool IsValidTimeZoneName(string name)
        {
            return TZConvert.KnownIanaTimeZoneNames.Contains(name);
        }

        #region System.Object methods.

        public override string ToString() => _name;

        #endregion
    }
}
