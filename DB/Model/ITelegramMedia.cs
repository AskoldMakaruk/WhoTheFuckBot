using System.IO;
using Telegram.Bot.Types;

namespace WhoTheFuckBot.DB.Model
{
    public abstract class TelegramMedia
    {
        public abstract string FileId { get; set; }
        public abstract byte[] File { get; set; }
        public static implicit operator InputMedia(TelegramMedia a)
        {
            return a.FileId == null? new InputMedia(new MemoryStream(a.File), "filename.png") : new InputMedia(a.FileId);
        }
    }

}