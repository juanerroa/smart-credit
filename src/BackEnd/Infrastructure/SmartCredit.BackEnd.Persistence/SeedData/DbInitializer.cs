using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartCredit.BackEnd.Persistence.Context;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Domain.Enums;

namespace AuctionService.Data.Seeding
{
    public class DbInitializer
    {
        public static void InitDb(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();

            if (dbContext == null)
            {
                Console.WriteLine("DatabaseContext is null. Check the service registration.");
                return;
            }

            // Aplicar migraciones pendientes
            dbContext.Database.Migrate();

            // Verificar si ya hay datos
            if (dbContext.CreditCards.Any())
            {
                Console.WriteLine("Already have data - no need to seed");
                return;
            }

            // Creamos los procedimientos almacenados
            RunSqlScripts(dbContext);

            // Generamos data inicial y configuraciones
            SeedData(dbContext);
        }

        private static void SeedData(DatabaseContext dbContext)
        {
            // Crear datos iniciales
            var users = new[]
            {
                new User
                {
                    FullName = "Juan Erroa",
                    Email = "jerroa.career@gmail.com",
                    Address = "123 Main St",
                    Country = "USA",
                    State = "California",
                    City = "Los Angeles"
                },
                new User
                {
                    FullName= "Javier Martinez",
                    Email = "javier.martinez@example.com",
                    Address = "456 Elm St",
                    Country = "Canada",
                    State = "Ontario",
                    City = "Toronto"
                },
                new User
                {
                    FullName = "Oscar Chevez",
                    Email = "oscar.chevez@example.com",
                    Address = "789 Oak St",
                    Country = "UK",
                    State = "England",
                    City = "London"
                }
            };

            dbContext.Users.AddRange(users);
            dbContext.SaveChanges(); // Guardar para obtener los Id generados automáticamente

            var creditCards = new[]
            {
                new CreditCard
                {
                    UserId = users[0].Id, // Relación con usuario
                    CardNumber = "4826341739734277",
                    HolderName = users[0].FullName,
                    Balance = 0m,
                    CreditLimit = 2000.00m,
                    AvailableBalance = 2000.00m,
                    ClosingDay = 25,
                    Type = CreditCardType.Visa,
                    ConfigurableInterestRate = 25,
                    ConfigurableMinimumBalanceRate = 5
                },
                new CreditCard
                {
                    UserId = users[1].Id, // Relación con usuario
                    CardNumber = "5289675295660963",
                    HolderName = users[1].FullName,
                    Balance = 0m,
                    CreditLimit = 1800.00m,
                    AvailableBalance = 1800.00m,
                    ClosingDay = 15,
                    Type = CreditCardType.MasterCard,
                    ConfigurableInterestRate = 25,
                    ConfigurableMinimumBalanceRate = 5
                },
                new CreditCard
                {
                    UserId = users[2].Id, // Relación con usuario
                    CardNumber = "347219523030185",
                    HolderName = users[2].FullName,
                    Balance = 0m,
                    CreditLimit = 1500.00m,
                    AvailableBalance = 1500.00m,
                    ClosingDay = 20,
                    Type = CreditCardType.AmericanExpress,
                    ConfigurableInterestRate = 25,
                    ConfigurableMinimumBalanceRate = 5
                }
            };

            dbContext.CreditCards.AddRange(creditCards);
            dbContext.SaveChanges(); // Guardar para obtener los Id generados automáticamente

            Console.WriteLine("Database seeded successfully.");
        }

        private static void RunSqlScripts(DatabaseContext dbContext)
        {
            var scriptDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "init_database");

            if (!Directory.Exists(scriptDirectory))
            {
                Console.WriteLine($"Script directory '{scriptDirectory}' not found. Skipping SQL script execution.");
                return;
            }

            var scriptFiles = Directory.GetFiles(scriptDirectory, "*.sql");

            foreach (var scriptFile in scriptFiles)
            {
                try
                {
                    var script = File.ReadAllText(scriptFile);
                    dbContext.Database.ExecuteSqlRaw(script);
                    Console.WriteLine($"Executed script: {Path.GetFileName(scriptFile)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing script {Path.GetFileName(scriptFile)}: {ex.Message}");
                }
            }
        }
    }
}
