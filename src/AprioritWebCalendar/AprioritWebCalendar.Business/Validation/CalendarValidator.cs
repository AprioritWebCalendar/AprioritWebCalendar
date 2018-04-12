using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Data.Interfaces;
using AprioritWebCalendar.Data.Models;
using AprioritWebCalendar.Infrastructure.Extensions;
using AprioritWebCalendar.ViewModel.Calendar;

namespace AprioritWebCalendar.Business.Validation
{
    public class CalendarValidator : ICalendarValidator
    {
        private readonly IRepository<Calendar> _calendarRepository;

        public CalendarValidator(IRepository<Calendar> calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<IEnumerable<ValidationResult>> ValidateCreateAsync(CalendarRequestModel model, int ownerId)
        {
            var errors = new List<ValidationResult>();

            if (await _calendarRepository.AnyAsync(c => c.Name.Equals(model.Name) && c.OwnerId == ownerId))
            {
                errors.AddError($"You already have a calendar with name {model.Name}.", "Name");
            }

            return errors;
        }

        public async Task<IEnumerable<ValidationResult>> ValidateUpdateAsync(int calendarId, CalendarRequestModel model, int ownerId)
        {
            var errors = new List<ValidationResult>();
            
            if (await _calendarRepository.AnyAsync(c => c.Name.Equals(model.Name) && c.OwnerId == ownerId && c.Id != calendarId))
            {
                errors.AddError($"You already have a calendar with name {model.Name}.", "Name");
            }

            return errors;
        }
    }
}
