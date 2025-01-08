using SmartCredit.FrontEnd.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.Contracts.Repositories
{
    public interface ICreditCardRepository
    {
        public Task<CreditCardViewModel> GetById(Guid? creditCardId);
        public Task<List<CreditCardViewModel>> GetAll();
        public Task<PaymentViewModel> CalculatePayments(int creditCardId);
        public Task<CreditCardViewModel> AddUserAndCreditCardAsync(CreditCardViewModel creditCard);
        public Task<CreditCardStatementViewModel> GetCreditCardStatement(Guid creditCardId, int year, int month);
    }
}