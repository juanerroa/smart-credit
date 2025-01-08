using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartCredit.BackEnd.Domain.Contracts.Repositories;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Domain.CustomEntities;
using SmartCredit.BackEnd.Persistence.Context;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Persistence.Services.Repositories
{
    public class CreditCardRepository : ICreditCardRepository
    {

        private readonly DatabaseContext _context;

        public CreditCardRepository(DatabaseContext context){
            _context = context;
        }

        public async Task<Payment> CalculatePayments(int creditCardId)
        {
             return await _context.Set<Payment>()
            .FromSqlRaw("EXEC [dbo].[CalculatePayments] @CreditCardId = {0}", creditCardId)
            .FirstOrDefaultAsync();
        }

        public async Task<List<CreditCard>> GetAll()
        {
            return await _context.CreditCards
            .FromSqlRaw("EXEC [dbo].[GetCreditCards]")
            .ToListAsync();
        }

        public async Task<CreditCard> GetById(Guid? creditCardId)
        {
            var response = await _context.CreditCards
                .FromSqlRaw("EXEC [dbo].[GetCreditCards] @CreditCardId = {0}", creditCardId)
                .ToListAsync();

            return response.FirstOrDefault();
        }

        public async Task<CreditCard> AddUserAndCreditCardAsync(User user, CreditCard creditCard)
        {
            // Define the parameters for the stored procedure
            var parameters = new object[]
            {
                user.FullName,
                user.Address,
                user.City,
                user.State,
                user.Country,
                user.Email,
                creditCard.CardNumber,
                creditCard.HolderName,
                creditCard.CreditLimit,
                creditCard.Balance,
                creditCard.AvailableBalance,
                creditCard.ClosingDay,
                (int)creditCard.Type,
                creditCard.ConfigurableInterestRate,
                creditCard.ConfigurableMinimumBalanceRate
            };

            // Call the stored procedure
            var creditCards = await _context.CreditCards
                .FromSqlRaw(
                    "EXEC [dbo].[AddUserAndCreditCard] " +
                    "@FullName = {0}, @Address = {1}, @City = {2}, @State = {3}, @Country = {4}, " +
                    "@Email = {5}, @CardNumber = {6}, @HolderName = {7}, @CreditLimit = {8}, " +
                    "@Balance = {9}, @AvailableBalance = {10}, @ClosingDay = {11}, @CreditCardType = {12}, " +
                    "@ConfigurableInterestRate = {13}, @ConfigurableMinimumBalanceRate = {14}",
                    parameters)
                .ToListAsync();

            // Return the first credit card created (or null if none)
            return creditCards.FirstOrDefault();
        }

        public async Task<CreditCardStatement> GetCreditCardStatement(Guid creditCardId, int year, int month)
        {
            var response = await _context.Set<CreditCardStatement>()
            .FromSqlRaw("EXEC [dbo].[GetCreditCardStatement] @CreditCardId = {0}, @Year = {1}, @Month = {2}", creditCardId, year, month)
            .ToListAsync();

            return response.FirstOrDefault();
        }
    }
}