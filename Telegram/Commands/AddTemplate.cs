using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Telegram.Bot;
namespace WhoTheFuckBot.Telegram.Commands
{
    public class AddTemplateCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "Add template") res = 2;
            return res;
        }
        public override Response Execute(Message message, Client client, Account account)
        {
            account.Status = AccountStatus.AddTemplate;
            return Response.TextMessage(account, "Send me image with zones for text");
        }
    }
}