using FluentValidation;
using SmartCredit.BackEnd.Application.Features.CreditCards.Queries;
using SmartCredit.BackEnd.Application.Features.Transactions.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.Transactions.Validators
{
    public class AddPaymentValidator : AbstractValidator<AddPayment>
    {
        public AddPaymentValidator()
        {
            RuleFor(x => x.CreditCardId)
            .NotEmpty().WithMessage("El ID de la tarjeta de crédito no puede estar vacío.")
            .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("El ID de la tarjeta debe ser un GUID válido.");

            RuleFor(x => x.Date)
                .Must(date => date.Year >= DateTime.Now.Year)
                .WithMessage("El año debe ser debe coincidir con el actual.");

            // Se auto genera...
            //RuleFor(x => x.Description)
            //    .NotEmpty()
            //    .WithMessage("La descripción no puede estar vacía.");

            // Quiero que sea mayor a 0
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("El monto debe ser mayor a 0.");
        }
    }
}
