using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }

        public SamuraiContext(IConfiguration config)
        {
            _config = config;
            Console.WriteLine("called");
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Port = Int32.Parse(_config.GetSection("ConnectionStrings")["pgPort"]);
            ;
            builder.Database = _config.GetConnectionString("pgDb");
            builder.Host = _config.GetConnectionString("pgHost");
            builder.Username = _config.GetConnectionString("pgUser");
            // from user-secret
            builder.Password = _config.GetSection("Secrets")["Samurai"];
            
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseNpgsql(
                    builder.ConnectionString
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //example of Fluent API usage
            modelBuilder.Entity<SamuraiBattle>().HasKey(s => new {s.SamuraiId, s.BattleId});
            // use Fluent API to create the table name if a DbSet isn't created, eg if you don't want/need to interact
            // directly with the table
            modelBuilder.Entity<Horse>().ToTable("Horses");
        }
    }
}