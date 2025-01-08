using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Wrappers;
using SmartCredit.BackEnd.Domain.Contracts;
using SmartCredit.BackEnd.Domain.Interfaces;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Queries
{
    public class GetCreditCardsList : IRequest<DataResponse<IEnumerable<CreditCardDTO>>>
    {
        public class GetCreditCardsListHandler : IRequestHandler<GetCreditCardsList, DataResponse<IEnumerable<CreditCardDTO>>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetCreditCardsListHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<IEnumerable<CreditCardDTO>>> Handle(GetCreditCardsList request, CancellationToken cancellationToken)
            {
                var cards = await _uow.CreditCardRepository.GetAll();
                var cardsDTO = _mapper.Map<IEnumerable<CreditCardDTO>>(cards);

                return new DataResponse<IEnumerable<CreditCardDTO>>(cardsDTO, 200);
            }
        }
    }

}