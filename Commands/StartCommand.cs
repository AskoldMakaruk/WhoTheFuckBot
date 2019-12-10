using BotFramework;
using BotFramework.Bot;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class StartCommand : Command
    {
        public override Response Run(Account account, Message message, Client client)
        {
            return new Response().SendTextMessage(account, "Send any text to receive picture.\n\nUse /set to change template.");
        }

        public override bool Suitable(Message message) => message.Text == "/start";
    }
}