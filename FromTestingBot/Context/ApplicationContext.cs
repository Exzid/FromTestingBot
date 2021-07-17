using Microsoft.EntityFrameworkCore;
using Order.Models;
using System.Configuration;

namespace Order.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.AppSettings.Get("pgsqlConStr"));
        }
    }
}
