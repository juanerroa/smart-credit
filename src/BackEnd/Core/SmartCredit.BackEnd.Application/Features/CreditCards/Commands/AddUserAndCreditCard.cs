using AutoMapper;
using MediatR;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Helpers;
using SmartCredit.BackEnd.Application.Wrappers;
using SmartCredit.BackEnd.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Commands
{
    public class AddUserAndCreditCard : IRequest<DataResponse<CreditCardDTO>>
    {
        public UserDTO User { get; set; }
        public CreditCardDTO Card { get; set; }

        public class GetByPeriodHandler : IRequestHandler<AddUserAndCreditCard, DataResponse<CreditCardDTO>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public GetByPeriodHandler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<DataResponse<CreditCardDTO>> Handle(AddUserAndCreditCard request, CancellationToken cancellationToken)
            {
                var userEntity = _mapper.Map<Domain.Entities.User>(request.User);
                var cardEntity = _mapper.Map<Domain.Entities.CreditCard>(request.Card);

                cardEntity.Type = CreditCardHelpers.GetCreditCardType(cardEntity.CardNumber) ?? Domain.Enums.CreditCardType.Desconocida;
                var transactions = await _uow.CreditCardRepository.AddUserAndCreditCardAsync(userEntity, cardEntity);
                var transactionsDTO = _mapper.Map<CreditCardDTO>(transactions);

                return new DataResponse<CreditCardDTO>(transactionsDTO, 200);
            }
        }
    }

}
