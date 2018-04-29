using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<EventUser>> GetEventsToNotifyAsync(DateTime dateTime);
    }
}
