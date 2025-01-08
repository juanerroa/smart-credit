using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartCredit.BackEnd.Domain.Contracts.Repositories;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Domain.Enums;
using SmartCredit.BackEnd.Domain.CustomEntities;
using SmartCredit.BackEnd.Persistence.Context;

namespace SmartCredit.BackEnd.Persistence.Services.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly DatabaseContext _context;

        public TransactionRepository(DatabaseContext context){
            _context = context;
        }

        public async Task<Transaction> AddPayment(Guid creditCardId, decimal amout, DateTime date)
        {
            var response = await _context.Transactions
            .FromSqlRaw("EXEC [dbo].[AddPayment] @CreditCardId = {0}, @Amount = {1}, @Date = {2}", creditCardId, amout, date)
            .ToListAsync();

            return response.FirstOrDefault();
        }

        public async Task<Transaction> AddPurchase(Guid creditCardId, string description, decimal amout, DateTime date)
        {
            var response = await _context.Transactions
            .FromSqlRaw("EXEC [dbo].[AddPurchase] @CreditCardId = {0}, @Description = {1}, @Amount = {2}, @Date = {3}", creditCardId, description, amout, date)
            .ToListAsync();

            return response.FirstOrDefault();
        }

        public async Task<List<Transaction>> GetByPeriod(Guid creditCardId, int year, int month, TransactionType? type)
        {
            return await _context.Transactions
            .FromSqlRaw("EXEC [dbo].[GetTransactionsByPeriod] @CreditCardId = {0}, @Year = {1}, @Month = {2}, @Type = {3}", creditCardId, year, month, type)
            .ToListAsync();
        }
    }
}