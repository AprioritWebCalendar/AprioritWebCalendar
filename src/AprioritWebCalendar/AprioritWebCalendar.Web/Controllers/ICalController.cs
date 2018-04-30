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

namespace AprioritWebCalendar.Web.Controllers
{
    [Route("api/iCal")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Produces("application/json")]
    public class ICalController : Controller
    {
        const string ICAL_MIME = "text/calendar";

        private readonly ICalendarService _calendarService;
        private readonly ICalendarExportService _calendarExportService;

        public ICalController(ICalendarService calendarService, ICalendarExportService calendarExportService)
        {
            _calendarService = calendarService;
            _calendarExportService = calendarExportService;
        }

        [HttpGet("Export/{id}")]
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
    }
}
