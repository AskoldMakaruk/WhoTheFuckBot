using BotFramework;
using BotFramework.Bot;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class SetTemplateCommand : Command
    {
        public override Response Run(Account account, Message message, Client client)
        {
            if (message.Text.Length <= 4)
            {
                return new Response().SendTextMessage(account, @"Use this command to set template text. $ is the place where your message will be.
Example:
/set Who the fuck is $?
will become
Who the fuck is <your text here>");
            }
            var template = message.Text[4..];
            account.TemplateText = template;
            account.Controller.SaveChanges();
            return new Response().SendTextMessage(account, "New template set");
        }

        public override bool Suitable(Message message) => message.Text != null && message.Text.StartsWith("/set");

    }
}