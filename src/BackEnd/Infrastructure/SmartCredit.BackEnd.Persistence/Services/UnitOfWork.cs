using System;
using SmartCredit.BackEnd.Domain.Contracts;
using SmartCredit.BackEnd.Domain.Contracts.Repositories;
using SmartCredit.BackEnd.Domain.Interfaces;
using SmartCredit.BackEnd.Persistence.Context;
using SmartCredit.BackEnd.Persistence.Services.Repositories;

namespace SmartCredit.BackEnd.Persistence.Services
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DatabaseContext _context;
        private ICreditCardRepository _creditCardRepository;
        private ITransactionRepository _transactionRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }


        // Implementación de repositorios (lazy initialization):
        /*=================================================================*/
        public ICreditCardRepository CreditCardRepository => _creditCardRepository ??= new CreditCardRepository(_context);
        public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_context);

        /*=================================================================*/


        //Metodos transaccionales para hacer commit en transacciones y liberación del DbContext.

        // Se utilizaran Procedimientos Almacenados
        //public async Task<int> CommitAsync()
        // {
        //     return await _context.SaveChangesAsync();
        // }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
