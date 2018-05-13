using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramSetWebHookResponse
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("ok")]
        public bool IsOk { get; set; }

        [JsonProperty("result")]
        public bool Result { get; set; }
    }
}
