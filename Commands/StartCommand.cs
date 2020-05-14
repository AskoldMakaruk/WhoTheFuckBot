using BotFramework.Bot;
using BotFramework.Responses;
using MemeBot.DB.Model;
using Telegram.Bot.Types;

namespace MemeBot.Telegram.Commands
{
    public class StartCommand : StaticCommand
    {
        public override Response Execute(Account account, Message message, Client client)
        {
            return new Response(new TextMessage(account, "Send any text to receive picture.\n\nUse /set to change template."));
        }

        public override bool Suitable(Message message) => message.Text == "/start";
    }
}