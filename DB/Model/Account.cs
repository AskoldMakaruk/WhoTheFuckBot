using System.ComponentModel.DataAnnotations.Schema;
using Telegram.Bot.Types;

namespace MemeBot.DB.Model
{
    public class Account
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public string TemplateText { get; set; }

        [NotMapped]
        public Controller Controller { get; set; }

        [NotMapped]
        public Meme CurrentMeme { get; set; }

        public static implicit operator ChatId(Account a) => a.ChatId;
    }
}