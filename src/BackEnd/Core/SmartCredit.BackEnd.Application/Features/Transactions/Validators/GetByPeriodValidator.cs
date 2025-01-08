using FluentValidation;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using SmartCredit.BackEnd.Application.Features.Transactions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.Transactions.Validators
{
    public class GetByPeriodValidator : AbstractValidator<GetByPeriod>
    {
        public GetByPeriodValidator()
        {
            RuleFor(x => x.CreditCardId)
                .NotEmpty().WithMessage("El CreditCardId es requerido.");

            RuleFor(x => x.Year)
                .NotNull()
                .NotEmpty()
                .WithMessage("El Year es requerido.");

            RuleFor(x => x.Month)
                .NotNull()
                .NotEmpty()
                .WithMessage("El Month es requerido.");
        }
    }
}
