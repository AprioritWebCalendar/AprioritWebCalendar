using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramChat
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public bool IsPrivate => Type.Equals("private");
    }
}
