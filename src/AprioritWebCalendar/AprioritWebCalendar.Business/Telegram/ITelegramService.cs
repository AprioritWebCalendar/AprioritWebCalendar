using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AprioritWebCalendar.Business.Telegram
{
    public interface ITelegramService
    {
        Task SetWebHookAsync();
        Task<TelegramMessage> SendMessageAsync(int telegramId, string text);
    }
}
