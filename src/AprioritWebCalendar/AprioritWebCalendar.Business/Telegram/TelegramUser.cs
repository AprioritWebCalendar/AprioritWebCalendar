using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramUser
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }
    }
}
