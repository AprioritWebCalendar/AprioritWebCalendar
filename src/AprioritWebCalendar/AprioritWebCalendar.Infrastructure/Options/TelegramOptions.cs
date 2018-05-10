using System.Collections.Generic;

namespace AprioritWebCalendar.Infrastructure.Options
{
    public class TelegramOptions
    {
        public string BotToken { get; set; }
        public string WebHookUrl { get; set; }
        public IEnumerable<string> AllowedUpdates { get; set; }
    }
}
