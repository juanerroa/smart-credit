using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartCredit.BackEnd.Application.DTOs;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using SmartCredit.BackEnd.Application.Features.Transactions.Commands;
using SmartCredit.BackEnd.Application.Features.Transactions.Queries;
using SmartCredit.BackEnd.Application.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<DataResponse<IEnumerable<TransactionDTO>>> GetByPeriod([FromBody] GetByPeriod request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<DataResponse<TransactionDTO>> AddPurchase([FromBody] AddPurchase request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        public async Task<DataResponse<TransactionDTO>> AddPayment([FromBody] AddPayment request)
        {
            return await _mediator.Send(request);
        }
    }
}
