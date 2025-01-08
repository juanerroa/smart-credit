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
    public class GetCreditCardById : IRequest<DataResponse<CreditCardDTO>>
    {
        public Guid Id { get; set; }

        public class GetCreditCardByIdHandler : IRequestHandler<GetCreditCardById, DataResponse<CreditCardDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetCreditCardByIdHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<CreditCardDTO>> Handle(GetCreditCardById request, CancellationToken cancellationToken)
            {
                var cards = await _uow.CreditCardRepository.GetById(request.Id);
                var cardsDTO = _mapper.Map<CreditCardDTO>(cards);

                return new DataResponse<CreditCardDTO>(cardsDTO, 200);
            }
        }
    }

}