using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using WhoTheFuckBot.DB;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.Controllers
{
    public class TelegramController
    {
        private TelegramContext Context;
        public void Start()
        {
            Context = new TelegramContext();
            Context.Database.EnsureCreated();
        }
        #region Account

        public static Dictionary<long, Account> Accounts = new Dictionary<long, Account>();
        public Account FromId(int id)
        {
            var account = Accounts.Values.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                account = Context.Accounts.Find(id);
                Accounts.Add(account.ChatId, account);
            }
            return account;
        }

        internal IEnumerable<Template> GetTemplates()
        {
            var rnd = new Random();
            return Context.Templates.ToArray().OrderBy(x => rnd.Next()).Take(10);
        }

        public Account FromMessage(Message message)
        {

            if (Accounts.ContainsKey(message.Chat.Id))
            {
                return Accounts[message.Chat.Id];
            }
            var account = Context.Accounts.Include(a => a.DbTemplates).FirstOrDefault(a => a.ChatId == message.Chat.Id);

            if (account == null)
            {
                account = CreateAccount(message);
            }
            if (!Accounts.ContainsKey(account.ChatId))
                Accounts.Add(account.ChatId, account);
            account.Language = message.From.LanguageCode;
            return account;
        }
        public Account FromQuery(CallbackQuery message)
        {
            var account = Context.Accounts.FirstOrDefault(a => a.ChatId == message.From.Id);
            if (account == null)
            {
                account = new Account()
                {
                ChatId = message.From.Id,
                Language = message.From.LanguageCode,
                Name = message.From.Username,
                Status = AccountStatus.Start
                };
                Context.Accounts.Add(account);
                SaveChanges();

            }
            return account;

        }
        private Account CreateAccount(Message message)
        {
            var account = new Account
            {
                ChatId = message.Chat.Id,
                Name = message.Chat.Username,
                Status = AccountStatus.Start,
            };
            if (message.Chat.Username == null)
                account.Name = message.Chat.FirstName + " " + message.Chat.LastName;
            Context.Accounts.Add(account);
            Context.SaveChanges();
            return account;
        }
        public void AddTemplate(Template template)
        {
            Context.Templates.Add(template);
            SaveChanges();
        }

        #endregion

        public void SaveChanges() => Context.SaveChanges();
    }
}