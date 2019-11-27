using BotFramework.Bot;
using Telegram.Bot.Args;
using WhoTheFuckBot.Telegram.Commands;

namespace WhoTheFuckBot
{
    public class WhoTheFuckClient : Client
    {
        public WhoTheFuckClient() : base()
        {
            Name = "WhoTheFuckBot";

            base.IDontCareJustMakeItWork(typeof(MainCommand).Assembly);
        }

        protected override string Token => "960195138:AAFuiefk1PQw6IEy0z0gL2MxKDZ79DYxSjU";

    }
}