using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BotApi.DB.Model;
using Telegram.Bot.Types;

namespace BotApi.DB
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
        public Account FromMessage(Message message)
        {
            var start = message.Text?.Length > "/start".Length && message.Text.StartsWith("/start");
            if (Accounts.ContainsKey(message.Chat.Id) && !start)
            {
                return Accounts[message.Chat.Id];
            }
            var account = Context.Accounts.FirstOrDefault(a => a.ChatId == message.Chat.Id);
            if (message.Text != null)
                if (start)
                {
                    var param = message.Text.Substring(7);
                    var base64EncodedBytes = Convert.FromBase64String(param);
                    param = Encoding.UTF8.GetString(base64EncodedBytes);
                    var p = param.Split("*");
                    switch (p[0])
                    {
                        case "in":
                            //create user if null
                            //add referal
                            break;
                    }
                }
            if (account == null)
            {
                account = CreateAccount(message);
            }
            if (!Accounts.ContainsKey(account.ChatId))
                Accounts.Add(account.ChatId, account);
            account.Language = message.From.LanguageCode;
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

        #endregion

        public IEnumerable<Image> GetImages()
        {
            return Context.Images.OrderByDescending(m => m.UsedCount).Take(30);
        }

        public void AddImage(Image img)
        {
            Context.Images.Add(img);
            SaveChanges();
        }

        public void SaveChanges() => Context.SaveChanges();
    }
}