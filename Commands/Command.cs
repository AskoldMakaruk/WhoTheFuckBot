using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public abstract class Command : IStaticCommand
    {
        public Response Execute(Message message, Client client)
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

            return Run(account, message, client);
        }
        public abstract Response Run(Account account, Message message, Client client);

        public abstract bool Suitable(Message message);

    }
}