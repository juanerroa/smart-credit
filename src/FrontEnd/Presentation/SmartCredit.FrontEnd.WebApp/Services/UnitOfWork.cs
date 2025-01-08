using SmartCredit.FrontEnd.WebApp.Contracts;
using SmartCredit.FrontEnd.WebApp.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HttpClient _httpClient;
        public UnitOfWork(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private ICreditCardRepository _creditCardRepository;
        private ITransactionsRepository _transactionsRepository;

        public ICreditCardRepository CreditCardRepository => _creditCardRepository ??= new CreditCardRepository(_httpClient);
        public ITransactionsRepository TransactionsRepository => _transactionsRepository ??= new TransactionsRepository(_httpClient);
    }
}