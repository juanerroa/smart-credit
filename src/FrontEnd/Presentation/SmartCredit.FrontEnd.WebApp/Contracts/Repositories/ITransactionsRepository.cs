using SmartCredit.FrontEnd.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.Contracts.Repositories
{
    public interface ITransactionsRepository
    {
        public Task<List<TransactionsViewModel>> GetByPeriod(Guid creditCardId, int year, int month, int? type);
        public Task<TransactionsViewModel> AddPayment(Guid creditCardId, decimal amout, DateTime date);
        public Task<TransactionsViewModel> AddPurchase(Guid creditCardId, string description, decimal amout, DateTime date);
    }
}