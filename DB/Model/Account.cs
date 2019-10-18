using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Telegram.Bot.Types;
using WhoTheFuckBot.Controllers;
namespace WhoTheFuckBot.DB.Model
{
    public class Account
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public AccountStatus Status { get; set; }
        public List<AccountToTemplate> DbTemplates { get; set; }

        [NotMapped]
        public List<Template> Templates => DbTemplates.Select(c => c.Template).ToList();

        public Template CurrentTemplate { get; set; }

        [NotMapped]
        public TelegramController Controller { get; set; }

        public static implicit operator ChatId(Account a) => a.ChatId;
    }

    public enum AccountStatus
    {
        Free,
        Start,
        AddTemplate,
    }
}