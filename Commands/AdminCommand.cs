using Telegram.Bot.Types;

namespace WhoTheFuckBot.Telegram.Commands
{
    public abstract class AdminCommand : Command
    {
        public override bool Suitable(Message message) => IsSuitable(message) && message.From.Id == 249258727;
        public abstract bool IsSuitable(Message message);
    }
}