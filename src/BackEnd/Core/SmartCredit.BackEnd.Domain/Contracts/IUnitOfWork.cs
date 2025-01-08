using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Contracts.Repositories;

namespace SmartCredit.BackEnd.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public ICreditCardRepository CreditCardRepository { get;}
        public ITransactionRepository TransactionRepository { get;}

    }
}