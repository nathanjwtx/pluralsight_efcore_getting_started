using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Battle> Battles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var secretConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddUserSecrets("1bc59482-6468-416d-b761-728f6f18b453")
                .Build();
            
            var builder = new NpgsqlConnectionStringBuilder();
            builder.Port = Int32.Parse(secretConfig.GetConnectionString("pgPort"));
            builder.Database = secretConfig.GetConnectionString("pgDb");
            builder.Host = secretConfig.GetConnectionString("pgHost");
            builder.Username = secretConfig.GetConnectionString("pgUser");
            // from user-secret
            builder.Password = secretConfig.GetSection("Secrets")["Samurai"];
            
            optionsBuilder.UseNpgsql(
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