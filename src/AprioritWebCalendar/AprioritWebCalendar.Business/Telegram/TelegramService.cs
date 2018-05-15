using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using AprioritWebCalendar.Infrastructure.Options;

namespace AprioritWebCalendar.Business.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramOptions _telegramOptions;
        private readonly HttpClient _httpClient;

        public TelegramService(IOptions<TelegramOptions> options)
        {
            _telegramOptions = options.Value;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"https://api.telegram.org/bot{_telegramOptions.BotToken}/")
            };
        }

        public async Task SetWebHookAsync()
        {
            var data = new
            {
                url = _telegramOptions.WebHookUrl + _telegramOptions.BotToken,
                allowed_updates = _telegramOptions.AllowedUpdates
            };

            var resp = await PerformTelegramRequest<TelegramSetWebHookResponse>("setWebhook", data);

            if (!resp.IsOk)
                throw new InvalidCastException();
        }

        public async Task<TelegramMessage> SendMessageAsync(int telegramId, string text)
        {
            var message = new
            {
                chat_id = telegramId,
                text,
                parse_mode = "html"
            };

            return await PerformTelegramRequest<TelegramMessage>("sendMessage", message);
        }

        private async Task<TResponse> PerformTelegramRequest<TResponse>(string method, object data)
        {
            var resp = await _httpClient.PostAsync(method, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));

            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException();

            var respContent = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(respContent);
        }
    }
}
