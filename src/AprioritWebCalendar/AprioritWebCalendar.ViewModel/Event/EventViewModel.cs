using System;
using System.Text;
using AprioritWebCalendar.Infrastructure.DataTypes;
using AprioritWebCalendar.ViewModel.Account;

namespace AprioritWebCalendar.ViewModel.Event
{
    public class EventViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; }
        public int? RemindBefore { get; set; }
        public bool IsPrivate { get; set; } = true;

        public LocationViewModel Location { get; set; }
        public PeriodViewModel Period { get; set; }
        public UserViewModel Owner { get; set; }

        public int? CalendarId { get; set; }
        public string Color { get; set; }
        public bool? IsReadOnly { get; set; }

        public string ToMessageString(TimeZoneInfoIana userTimeZone)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"<b>{Name}</b>");

            if (IsAllDay)
            {
                sb.AppendLine($"Start: {StartDate.Value.ToString("d")}");
                sb.AppendLine($"End: {EndDate.Value.ToString("d")}");
            }
            else
            {
                sb.AppendLine($"Start: {userTimeZone.ConvertFromUtc(StartDate.Value.AddMinutes(StartTime?.TotalMinutes ?? 0)).ToString("g")}");
                sb.AppendLine($"End: {userTimeZone.ConvertFromUtc(EndDate.Value.AddMinutes(EndTime?.TotalMinutes ?? 0)).ToString("g")}");
            }

            if (!string.IsNullOrEmpty(Location?.Description))
            {
                sb.AppendLine($"Location: {Location.Description}");
            }

            sb.AppendLine();
            return sb.ToString();
        }
    }
}
