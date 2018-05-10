using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;

namespace AprioritWebCalendar.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Telegram")]
    public class TelegramController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> WebHook()
        {
            return Ok();
        }
    }
}
