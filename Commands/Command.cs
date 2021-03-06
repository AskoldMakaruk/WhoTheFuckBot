using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using BotFramework.Responses;
using MemeBot.DB;
using MemeBot.DB.Model;
using Telegram.Bot.Types;

namespace MemeBot.Telegram.Commands
{
    public abstract class Command : MessageCommand
    {
        public override Response Execute(Message message, Client client)
        {
            Account account;
            if (Controller.Accounts.ContainsKey(message.From.Id))
            {
                account = Controller.Accounts[message.From.Id];
            }
            else
            {
                var controller = new Controller();
                controller.Start();

                account = controller.FromMessage(message);
                account.Controller = controller;
            }

            return Execute(account, message, client);
        }
        public abstract Response Execute(Account account, Message message, Client client);

    }
    public abstract class StaticCommand : Command, IStaticCommand { }
}