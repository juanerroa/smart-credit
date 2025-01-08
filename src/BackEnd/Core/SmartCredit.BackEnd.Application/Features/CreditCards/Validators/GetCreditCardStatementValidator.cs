using FluentValidation;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Validators
{
    public class GetCreditCardStatementValidator : AbstractValidator<GetCreditCardStatement>
    {
        public GetCreditCardStatementValidator()
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
