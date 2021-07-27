using Microsoft.EntityFrameworkCore;
using Order.Models;
using System;
using System.Configuration;

namespace Order.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Rate> Rate { get; set; }

        public ApplicationContext()
        {
            try 
            { 
                Database.EnsureCreated();
            }
            catch (Npgsql.NpgsqlException e)
            {
                //Добавить логи
                Console.WriteLine("catch in constructor!\n" + "Message pg: " + e.Message + "\n");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine("Ошибка Dns при подключении к бд \n ");
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConfigurationManager.AppSettings.Get("pgsqlConStr"));
        }
    }
}
