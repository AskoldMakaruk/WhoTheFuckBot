using BotFramework;
using BotFramework.Bot;
using BotFramework.Responses;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class StartCommand : StaticCommand
    {
        public override Response Execute(Account account, Message message, Client client)
        {
            return new Response().AddMessage(new TextMessage(account, "Send any text to receive picture.\n\nUse /set to change template."));
        }

        public override bool Suitable(Message message) => message.Text == "/start";
    }
}