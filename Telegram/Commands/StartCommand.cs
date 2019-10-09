using Telegram.Bot.Types;
using WhoTheFuckBot.DB.Model;
using WhoTheFuckBot.Telegram.Bot;

namespace WhoTheFuckBot.Telegram.Commands
{
    public class StartCommand : Command
    {
        public override int Suitability(Message message, Account account)
        {
            int res = 0;
            if (message.Text == "/start" || account.Status == AccountStatus.Start) res = 2;
            return res;
        }
        public override Response Execute(Message message, Client client, Account account)
        {
            account.Status = AccountStatus.Free;
            return Response.TextMessage(account, "Send text to create a meme or choose template", replyMarkup : MainKeyboard());
        }
    }
}