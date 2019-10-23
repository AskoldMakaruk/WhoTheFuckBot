using System;
using System.IO;
using BotApi.Telegram;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var token = File.ReadAllText("token.txt");
            var token = "823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA";
            WebHookController.Bot = new Client(token);
            var bot = WebHookController.Bot.Bot;

            var s = bot.GetWebhookInfoAsync().Result;
            System.Console.WriteLine(s.LastErrorMessage);
            bot.SetWebhookAsync("https://134.249.124.62.xip.io/WhoTheFuckBot", File.OpenRead("/etc/nginx/certificates/sample-echobot.pem")).Wait();
            s = bot.GetWebhookInfoAsync().Result;
            bot.StartReceiving();
            CreateHostBuilder(args).Build().RunAsync();
            while (true)
            {

                Console.ReadLine();
                //System.Console.WriteLine(bot.GetWebhookInfoAsync().Result.LastErrorMessage);

            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://localhost:8444")
                        .UseStartup<Startup>();
                });
        }
    }
}