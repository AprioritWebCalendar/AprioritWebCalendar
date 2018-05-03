using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AprioritWebCalendar.ViewModel.Calendar
{
    public class CalendarImportModel : CalendarRequestModel
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
