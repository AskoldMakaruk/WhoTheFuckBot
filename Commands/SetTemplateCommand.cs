using BotFramework;
using BotFramework.Bot;
using BotFramework.Responses;
using MemeBot.DB.Model;
using Telegram.Bot.Types;

namespace MemeBot.Telegram.Commands
{
    public class SetTemplateCommand : StaticCommand
    {
        public override Response Execute(Account account, Message message, Client client)
        {
            if (message.Text.Length <= 4)
            {
                return new Response(new TextMessage(account, @"Use this command to set template text. $ is the place where your message will be.
Example:
/set Who the fuck is $?
will become
Who the fuck is <your text here>"));
            }
            var template = message.Text[4..];
            account.TemplateText = template;
            account.Controller.SaveChanges();
            return new Response(new TextMessage(account, "New template set"));
        }

        public override bool Suitable(Message message) => message.Text != null && message.Text.StartsWith("/set");

    }
}