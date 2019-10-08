using Telegram.Bot.Types;
using WhoTheFuckBot.Controllers;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Queries
{
    public abstract class Query
    {
        public TelegramController Controller { get; set; }

        public abstract Response Execute(CallbackQuery message, Account account);
        public abstract bool IsSuitable(CallbackQuery message, Account account);
    }
}