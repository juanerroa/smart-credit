using AutoMapper;
using MediatR;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Wrappers;
using SmartCredit.BackEnd.Domain.CustomEntities;
using SmartCredit.BackEnd.Domain.Enums;
using SmartCredit.BackEnd.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Queries
{
    public class GetCreditCardStatement : IRequest<DataResponse<CreditCardStatementDTO>>
    {
        public Guid CreditCardId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public class GetCreditCardStatementHandler : IRequestHandler<GetCreditCardStatement, DataResponse<CreditCardStatementDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetCreditCardStatementHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<CreditCardStatementDTO>> Handle(GetCreditCardStatement request, CancellationToken cancellationToken)
            {
                var transactions = await _uow.CreditCardRepository.GetCreditCardStatement(request.CreditCardId, request.Year, request.Month);
                var transactionsDTO = _mapper.Map<CreditCardStatementDTO>(transactions);

                return new DataResponse<CreditCardStatementDTO>(transactionsDTO, 200);
            }
        }
    }
}
