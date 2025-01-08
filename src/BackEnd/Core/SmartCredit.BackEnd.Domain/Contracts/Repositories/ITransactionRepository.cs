using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Domain.Contracts.Repositories
{
    public interface ITransactionRepository
    {
        public Task<List<Transaction>> GetByPeriod(Guid creditCardId, int year, int month, TransactionType? type);
        public Task<Transaction> AddPayment(Guid creditCardId, decimal amout, DateTime date);
        public Task<Transaction> AddPurchase(Guid creditCardId, string description, decimal amout, DateTime date);
    }
}