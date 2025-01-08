using SmartCredit.FrontEnd.WebApp.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.Contracts
{
    public interface IUnitOfWork
    {
        public ICreditCardRepository CreditCardRepository { get; }
        public ITransactionsRepository TransactionsRepository { get; }

    }
}