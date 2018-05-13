using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramMessageEntity
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        public bool IsBotCommand => Type.Equals("bot_command");
    }
}
