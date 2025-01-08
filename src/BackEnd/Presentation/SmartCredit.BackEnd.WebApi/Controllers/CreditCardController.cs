using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Features.CreditCards.Commands;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using SmartCredit.BackEnd.Application.Wrappers;

namespace SmartCredit.BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CreditCardController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CreditCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<DataResponse<IEnumerable<CreditCardDTO>>> GetAll()
        {
            return await _mediator.Send(new GetCreditCardsList());
        }

        [HttpGet("{Id}")]
        public async Task<DataResponse<CreditCardDTO>> GetById(Guid Id)
        {
            return await _mediator.Send(new GetCreditCardById { Id = Id});
        }

        [HttpPost]
        public async Task<DataResponse<CreditCardDTO>> AddUserAndCreditCard([FromBody] AddUserAndCreditCard request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<DataResponse<CreditCardStatementDTO>> GetCreditCardStatement([FromBody] GetCreditCardStatement request)
        {
            return await _mediator.Send(request);
        }
    }
}