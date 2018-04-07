using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.ViewModel.Calendar;

namespace AprioritWebCalendar.Business.Interfaces
{
    public interface ICalendarValidator
    {
        Task<IEnumerable<ValidationResult>> ValidateCreateAsync(CalendarRequestModel model, int ownerId);
        Task<IEnumerable<ValidationResult>> ValidateUpdateAsync(int calendarId, CalendarRequestModel model, int ownerId);
    }
}
