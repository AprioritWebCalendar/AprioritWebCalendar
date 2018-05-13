using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramMessageUpdate
    {
        [JsonProperty("update_id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public TelegramMessage Message { get; set; }
    }
}
