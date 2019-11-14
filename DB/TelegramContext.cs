using Microsoft.EntityFrameworkCore;
using WhoTheFuckBot.DB.Model;

namespace WhoTheFuckBot.DB
{
    public class TelegramContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data/WhoTheFuckBot.db");
        }
    }
}