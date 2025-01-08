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

namespace SmartCredit.BackEnd.Application.Features.Transactions.Queries
{
    public class GetByPeriod : IRequest<DataResponse<IEnumerable<TransactionDTO>>>
    {
        public Guid CreditCardId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public TransactionType? Type { get; set; }

        public class GetByPeriodHandler : IRequestHandler<GetByPeriod, DataResponse<IEnumerable<TransactionDTO>>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetByPeriodHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<IEnumerable<TransactionDTO>>> Handle(GetByPeriod request, CancellationToken cancellationToken)
            {
                var transactions = await _uow.TransactionRepository.GetByPeriod(request.CreditCardId, request.Year, request.Month, request.Type);
                var transactionsDTO = _mapper.Map<IEnumerable<TransactionDTO>>(transactions);

                return new DataResponse<IEnumerable<TransactionDTO>>(transactionsDTO, 200);
            }
        }
    }

}