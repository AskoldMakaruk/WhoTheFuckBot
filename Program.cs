using System;

namespace WhoTheFuckBot
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var client = new WhoTheFuckClient();
            client.OnLog += (c, m) => Console.WriteLine(m);

            Console.ReadLine();
        }
    }
}