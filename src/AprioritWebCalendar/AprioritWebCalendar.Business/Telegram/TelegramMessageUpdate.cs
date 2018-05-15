using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using AprioritWebCalendar.Infrastructure.Extensions;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramMessageUpdate : IValidatableObject
    {
        [JsonProperty("update_id")]
        public int Id { get; set; }

        [JsonProperty("message")]
        public TelegramMessage Message { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();

            if ((DateTime.UtcNow - Message.DateTime).TotalSeconds >= 100)
                errors.AddError("The message is too old.", nameof(Message.DateTime));

            if (!Message.IsBotCommand)
                errors.AddError("It is not a command.", nameof(Message.IsBotCommand));

            if (!Message.IsFromRealUser)
                errors.AddError("The bot does not work with bots.", nameof(Message.IsFromRealUser));

            if (!Message.IsFromPrivateChat)
                errors.AddError("The bot does not work in groups or public chats.", nameof(Message.IsFromPrivateChat));

            return errors;
        }
    }
}
