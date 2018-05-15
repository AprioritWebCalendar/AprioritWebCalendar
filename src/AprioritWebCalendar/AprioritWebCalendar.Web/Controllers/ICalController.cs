using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AprioritWebCalendar.Business.Interfaces;
using AprioritWebCalendar.Web.Extensions;
using AprioritWebCalendar.ViewModel.Calendar;
using AprioritWebCalendar.Web.Filters;
using AprioritWebCalendar.Business.DomainModels;

namespace AprioritWebCalendar.Web.Controllers
{
    [Route("api/iCal")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    [ExceptionHandler]
    public class ICalController : Controller
    {
        const string ICAL_MIME = "text/calendar";

        private readonly ICalendarService _calendarService;
        private readonly ICalendarValidator _calendarValidator;
        private readonly ICalendarExportService _calendarExportService;
        private readonly ICalendarImportService _calendarImportService;
        private readonly ICacheService _cacheService;

        public ICalController(
            ICalendarService calendarService, 
            ICalendarValidator calendarValidator,
            ICalendarExportService calendarExportService,
            ICalendarImportService calendarImportService,
            ICacheService cacheService)
        {
            _calendarService = calendarService;
            _calendarValidator = calendarValidator;
            _calendarExportService = calendarExportService;
            _calendarImportService = calendarImportService;
            _cacheService = cacheService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Export(int id)
        {
            var userId = this.GetUserId();

            if (!await _calendarService.IsOwnerOrSharedWithAsync(id, userId))
            {
                return this.BadRequestError("You do not have any permissions to this calendar.");
            }

            var domainCalendar = await _calendarService.GetCalendarReadyToExportAsync(id, userId);
            var ics = _calendarExportService.SerializeCalendar(_calendarExportService.ExportCalendar(domainCalendar));

            var stream = new MemoryStream(Encoding.ASCII.GetBytes(ics));
            return new FileStreamResult(stream, ICAL_MIME);
        }

        [HttpPost, ValidateApiModelFilter]
        public async Task<IActionResult> Import(CalendarImportModel model)
        {
            // TODO: Handle exceptions.

            if (!model.File.ContentType.Equals(ICAL_MIME))
                ModelState.AddModelError("File", $"Content-Type \"{model.File.ContentType}\" is not supported.");

            var userId = this.GetUserId();

            (await _calendarValidator.ValidateCreateAsync(model, userId)).ToModelState(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToStringEnumerable());

            string fileContent = null;

            try
            {
                using (var reader = new StreamReader(model.File.OpenReadStream()))
                {
                    fileContent = await reader.ReadToEndAsync();
                }
            }
            catch
            {
                return this.BadRequestError("Unable to read file.");
            }

            if (string.IsNullOrEmpty(fileContent))
                return this.BadRequestError("Unable to read file.");

            Calendar calendar = null;

            try
            {
                calendar = _calendarImportService.ConvertIntoDomain(_calendarImportService.DeserializeCalendar(fileContent));
            }
            catch
            {
                return this.BadRequestError("Unable to deserialize file.");
            }

            calendar.Owner = new User { Id = userId };
            calendar.Name = model.Name;
            calendar.Description = model.Description;
            calendar.Color = model.Color;

            var guid = Guid.NewGuid().ToString();
            _cacheService.SetItem(guid, calendar, 5);

            return Ok(new
            {
                Key = guid,
                Calendar = calendar
            });
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> SaveCalendar(string key)
        {
            var calendar = _cacheService.GetItem<Calendar>(key);

            if (calendar == null)
                return NotFound();

            if (this.GetUserId() != calendar.Owner.Id)
                return this.BadRequestError("You can't save a calendar of another user.");

            calendar = await _calendarImportService.SaveCalendarAsync(calendar);
            _cacheService.RemoveItem(key);
            return Ok(new { calendar.Id });
        }
    }
}
