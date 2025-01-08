using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Wrappers;
using SmartCredit.BackEnd.Domain.Contracts;
using SmartCredit.BackEnd.Domain.Enums;
using SmartCredit.BackEnd.Domain.Interfaces;

namespace SmartCredit.BackEnd.Application.Features.Transactions.Commands
{
    public class AddPurchase : IRequest<DataResponse<TransactionDTO>>
    {
        public Guid CreditCardId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }


        public class GetByPeriodHandler : IRequestHandler<AddPurchase, DataResponse<TransactionDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetByPeriodHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<TransactionDTO>> Handle(AddPurchase request, CancellationToken cancellationToken)
            {
                var transactions = await _uow.TransactionRepository.AddPurchase(request.CreditCardId, request.Description, request.Amount, request.Date);
                var transactionsDTO = _mapper.Map<TransactionDTO>(transactions);

                return new DataResponse<TransactionDTO>(transactionsDTO, 200);
            }
        }
    }

}