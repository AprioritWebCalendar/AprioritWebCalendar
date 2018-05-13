using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramMessage
    {
        [JsonProperty("message_id")]
        public int Id { get; set; }

        [JsonProperty("from")]
        public TelegramUser From { get; set; }

        [JsonProperty("chat")]
        public TelegramChat Chat { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("date")]
        public int DateUnix { get; set; }

        [JsonProperty("entities")]
        public IEnumerable<TelegramMessageEntity> Entities { get; set; }

        public bool IsFromPrivateChat => From.Id == Chat.Id && Chat.IsPrivate;

        public bool IsFromRealUser => !From.IsBot;

        public bool IsBotCommand
        {
            get
            {
                var commandsCount = Entities?.Count(e => e.IsBotCommand);
                return commandsCount == 1;
            }
        }

        public DateTime DateTime
        {
            get => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(DateUnix);
        }

        public string GetCommandWithoutSlash()
        {
            return Text.Substring(1);
        }
    }
}
