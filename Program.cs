using System;
using System.IO;
using WhoTheFuckBot.Telegram;
using WhoTheFuckBot.Telegram.Bot;

namespace WhoTheFuckBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var token = File.ReadAllText("token.txt");
            var token = "823973981:AAGYpq1Eyl_AAYGXLeW8s28uCH89S7fsHZA";
            var bot = new Client(token);
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}