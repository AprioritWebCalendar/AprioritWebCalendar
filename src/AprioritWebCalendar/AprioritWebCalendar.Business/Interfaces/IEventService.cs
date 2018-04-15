using AprioritWebCalendar.Business.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(int id);
        Task<IEnumerable<Event>> GetEventsAsync(int calendarId, DateTime start, DateTime end);
        Task<int> CreateEventAsync(Event domainEvent, int calendarId);
    }
}
