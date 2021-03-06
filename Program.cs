using MemeBot.Telegram.Commands;
using Serilog;

namespace MemeBot
{
    public static class Program
    {

#if RELEASE
        static string Token => "960195138:AAFuiefk1PQw6IEy0z0gL2MxKDZ79DYxSjU";
#endif
#if DEBUG
        static string Token => "823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA";
#endif
        public static void Main()
        {
            var client = new BotFramework.Bot.BotBuilder()
                .UseAssembly(typeof(MainCommand).Assembly)
                .WithName("MemeBot")
                .WithToken(Token)
                .UseLogger(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("log.log")
                    .CreateLogger())
                .Build();

            client.Run();
        }
    }
}