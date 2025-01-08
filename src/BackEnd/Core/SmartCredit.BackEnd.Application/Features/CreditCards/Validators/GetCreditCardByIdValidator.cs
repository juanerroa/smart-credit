using FluentValidation;
using SmartCredit.BackEnd.Application.Features.CreditCards.Commands;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Validators
{
    public class GetCreditCardByIdValidator : AbstractValidator<GetCreditCardById>
    {
        public GetCreditCardByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty().
                WithMessage("El Id es requerido.");
        }
    }
}
