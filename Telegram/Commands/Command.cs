using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WhoTheFuckBot.Controllers;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Telegram.Bot;

namespace WhoTheFuckBot.Telegram.Commands
{
    public abstract class Command
    {
        // 0 not at all
        // 1 just handle reply
        // 2 main condition is true
        public abstract int Suitability(Message message, Account account);
        public abstract Response Execute(Message message, Client client, Account account);

        public virtual bool Canceled(Message message, Account account)
        {
            return message.Text.ToLower().Equals("cancel") ||
                message.Text.ToLower().Equals("/cancel");
        }

        public virtual Response Relieve(Message message, Client client, Account account)
        {
            return new MainCommand().Execute(message, client, account);
        }

        public static KeyboardButton[][] MainKeyboard()
        {
            return new []
            {
                new KeyboardButton[]
                {
                    "Resize Image"
                }
            };
        }
    }
}