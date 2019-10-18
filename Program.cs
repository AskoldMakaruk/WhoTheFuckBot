using System.IO;
using BotApi.Telegram;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var token = File.ReadAllText("token.txt");
            token = "823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA";
            var bot = new Client(token);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}