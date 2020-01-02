using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class SendUpdateCommand : AdminCommand
    {
        public override bool IsSuitable(Message message) => message.Text.StartsWith("/sendupdate");

        public override Response Run(Account account, Message message, Client client)
        {
            return new Response(new WaitForUpdateTextCommand());
        }
    }

    public class WaitForUpdateTextCommand : ICommand
    {
        public Response Execute(Message message, Client client)
        {
            throw new System.NotImplementedException();
        }
    }
}