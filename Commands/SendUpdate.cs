using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using BotFramework.Responses;
using MemeBot.DB.Model;
using Telegram.Bot.Types;

namespace MemeBot.Telegram.Commands
{
    public class SendUpdateCommand : AdminCommand
    {
        public override bool IsSuitable(Message message) => message.Text.StartsWith("/sendupdate");

        public override Response Execute(Account account, Message message, Client client)
        {
            return new Response(new WaitForUpdateTextCommand());
        }
    }

    public class WaitForUpdateTextCommand : MessageCommand
    {
        public override Response Execute(Message message, Client client)
        {
            throw new System.NotImplementedException();
        }

        public override bool Suitable(Message message)
        {
            throw new System.NotImplementedException();
        }
    }
}