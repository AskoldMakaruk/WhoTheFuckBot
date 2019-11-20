using BotFramework;
using BotFramework.Bot;
using BotFramework.Commands;
using Telegram.Bot.Types;
namespace WhoTheFuckBot.Telegram.Commands
{
    public class SetBitmapPath : IStaticCommand
    {
        static string _path;
        public static string BitmapPath
        {
            get
            {
                if (_path == null)
                {
                    _path = System.IO.File.Exists("imagepath.txt") ? System.IO.File.ReadAllText("imagepath.txt") : null;
                }
                return _path;
            }
            set
            {
                _path = value;
                System.IO.File.WriteAllText("imagepath.txt", value);
            }
        }
        public Response Execute(Message message, Client client)
        {
            var newpath = message.Text.Substring(5);
            if (System.IO.File.Exists(newpath))
            {
                BitmapPath = newpath;
                return new Response(this, new MainCommand()).SendTextMessage(message.Chat.Id, "Path set");
            }
            else return new Response(this, new MainCommand()).SendTextMessage(message.Chat.Id, "You trying to fuck with me or what?");

        }

        public bool Suitable(Message message) => message.Text != null && message.Text.StartsWith("/set") && message.Chat.Id == 249258727;

    }
}