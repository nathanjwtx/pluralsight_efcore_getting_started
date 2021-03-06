﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace ConsoleApp
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            // AddSamurai();
            // GetSamurais("After Add:");
            // InsertMultipleSamurais();
            GetSamuraisSimpler();
            // Console.WriteLine("Press any key...");
            // Console.ReadKey();
            QueryFilters();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai {Name = "Bob"};
            var samurai2 = new Samurai {Name = "Dave"};
            _context.Samurais.Add(samurai);
            _context.Samurais.Add(samurai2);
            // could also do
            // _context.Samurais.AddRange(samurai, samurai2);
            // could also construct a list and pass into AddRange
            _context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai {Name = "Charlie"};
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = _context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }

        private static void GetSamuraisSimpler()
        {
            // ToList, and similar, are referred to as query executors/execution method
            
            // one line
            // var samurais = _context.Samurais.ToList();
            
            // it can be split into multiple lines
            // var query = _context.Samurais;
            // var samurais = query.ToList();
            
            // or it can be executed as a loop
            var query = _context.Samurais;
            foreach (var samurai in query)
            {
                Console.WriteLine(samurai.Name);
            }
            // downside to this method is that the connection is held open until the enumeration has completed
        }

        private static void InsertVariousTypes()
        {
            var samurai = new Samurai {Name = "Kikuchio"};
            var clan = new Clan {ClanName = "Imperial Clan"};
            _context.AddRange(samurai, clan);
            _context.SaveChanges();
        }

        private static void QueryFilters()
        {
            var name = "Sampson";
            var filter = "S%";
            // var samurais = _context.Samurais
            //     .Where(s => EF.Functions.Like(s.Name, filter)).ToList();

            var samurai = _context.Samurais.FirstOrDefault(s => s.Id == 2);
            
            // must be ordered in order to use LastOrDefault
            var last = _context.Samurais.OrderBy(s => s.Id).LastOrDefault(s => s.Name == name);
        }
    }
}