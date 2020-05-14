using MemeBot.DB.Model;
using Microsoft.EntityFrameworkCore;

namespace MemeBot.DB
{
    public class TelegramContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Meme> Memes { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=MemeBot.db");
        }
    }
}