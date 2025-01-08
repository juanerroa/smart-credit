using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Entities;
using SmartCredit.BackEnd.Domain.CustomEntities;

namespace SmartCredit.BackEnd.Domain.Contracts.Repositories
{
    public interface ICreditCardRepository
    {
        public Task<CreditCard> GetById(Guid? creditCardId);
        public Task<List<CreditCard>> GetAll();
        public Task<Payment> CalculatePayments(int creditCardId);
        public Task<CreditCard> AddUserAndCreditCardAsync(User user, CreditCard creditCard);
        public Task<CreditCardStatement> GetCreditCardStatement(Guid creditCardId, int year, int month);

    }
}