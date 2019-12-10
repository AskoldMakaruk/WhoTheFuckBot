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
            IDontCareJustMakeItWork(typeof(MainCommand).Assembly);
        }
#if RELEASE
        protected override string Token => "960195138:AAFuiefk1PQw6IEy0z0gL2MxKDZ79DYxSjU";
#endif
#if DEBUG
        protected override string Token => "823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA";
#endif

    }
}