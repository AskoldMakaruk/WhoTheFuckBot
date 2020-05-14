using System;
using System.Collections.Generic;
using System.Linq;
using MemeBot.DB.Model;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace MemeBot.DB
{
    public class Controller : IDisposable
    {
        private TelegramContext Context;
        public void Start()
        {
            Context = new TelegramContext();
            Context.Database.EnsureCreated();
            Context.Database.Migrate();
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
            if (Accounts.ContainsKey(message.Chat.Id))
            {
                return Accounts[message.Chat.Id];
            }
            var account = Context.Accounts.FirstOrDefault(a => a.ChatId == message.Chat.Id);

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
                Name = message.From.Username
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
                Name = message.Chat.Username
            };
            if (message.Chat.Username == null)
                account.Name = message.Chat.FirstName + " " + message.Chat.LastName;
            Context.Accounts.Add(account);
            Context.SaveChanges();
            return account;
        }

        #endregion

        public void AddMeme(Meme meme)
        {
            Context.Memes.Add(meme);
            SaveChanges();
        }

        public void LikeMeme(int memeId, int accountId)
        {
            var like = Context.Likes.FirstOrDefault(l => l.AccountId == accountId && l.MemeId == memeId);
            var meme = Context.Memes.Find(memeId);
            if (like == null)
            {
                meme.Likes++;
                Context.Likes.Add(new Like { AccountId = accountId, MemeId = memeId });
            }
            else
            {
                meme.Likes--;
                Context.Likes.Remove(like);
            }

            SaveChanges();
        }

        public int CountLikes(int memeId)
        {
            return Context.Likes.Count(l => l.MemeId == memeId);
        }

        public void SaveChanges() => Context.SaveChanges();
        public void Dispose() => Context.Dispose();

    }
}