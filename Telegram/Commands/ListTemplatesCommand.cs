using System.Linq;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Telegram.Bot;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class ListTemplatesCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "List templates") res = 2;
            return res;
        }
        public override Response Execute(Message message, Client client, Account account)
        {
            client.Bot.SendMediaGroupAsync(account, account.Controller.GetTemplates().Select(c => new InputMedia()))
            return Response.TextMessage(account, string.Join("\n", account.Controller.GetTemplates().Select(t => $"{t.Name} - /{t.Id}")));
        }
    }
}