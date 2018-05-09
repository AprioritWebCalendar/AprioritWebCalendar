using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Infrastructure.DataTypes;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ISettingsService
    {
        Task<TimeZoneInfoIana> GetTimeZoneAsync(int userId);
        Task SetTimeZoneAsync(int userId, TimeZoneInfoIana timeZone);
    }
}
