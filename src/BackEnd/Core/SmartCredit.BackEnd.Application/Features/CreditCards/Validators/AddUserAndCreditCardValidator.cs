using FluentValidation;
using SmartCredit.BackEnd.Application.Features.CreditCards.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.Features.CreditCards.Validators
{
    public class AddUserAndCreditCardValidator : AbstractValidator<AddUserAndCreditCard>
    {
        public AddUserAndCreditCardValidator()
        {
            // Validación para User.FullName
            RuleFor(x => x.User.FullName)
                .NotEmpty().WithMessage("El nombre completo es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre completo no debe exceder los 100 caracteres.");

            // Validación para User.Address
            RuleFor(x => x.User.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no debe exceder los 200 caracteres.");

            // Validación para User.City
            RuleFor(x => x.User.City)
                .NotEmpty().WithMessage("La ciudad es obligatoria.")
                .MaximumLength(100).WithMessage("La ciudad no debe exceder los 100 caracteres.");

            // Validación para User.State
            RuleFor(x => x.User.State)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .MaximumLength(100).WithMessage("El estado no debe exceder los 100 caracteres.");

            // Validación para User.Country
            RuleFor(x => x.User.Country)
                .NotEmpty().WithMessage("El país es obligatorio.")
                .MaximumLength(100).WithMessage("El país no debe exceder los 100 caracteres.");

            // Validación para User.Email
            RuleFor(x => x.User.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido.");

            // Validación para Card.CardNumber
            RuleFor(x => x.Card.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta es obligatorio.")
                .CreditCard().WithMessage("Debe proporcionar un número de tarjeta de crédito válido.");

            // Validación para Card.HolderName
            RuleFor(x => x.Card.HolderName)
                .NotEmpty().WithMessage("El nombre del titular es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del titular no debe exceder los 100 caracteres.");

            // Validación para Card.CreditLimit
            RuleFor(x => x.Card.CreditLimit)
                .GreaterThan(0).WithMessage("El límite de crédito debe ser mayor a 0.");

            // Validación para Card.Balance
            RuleFor(x => x.Card.Balance)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo no puede ser negativo.");

            // Validación para Card.AvailableBalance
            RuleFor(x => x.Card.AvailableBalance)
                .GreaterThanOrEqualTo(0).WithMessage("El saldo disponible no puede ser negativo.");

            // Validación para Card.ClosingDay
            RuleFor(x => x.Card.ClosingDay)
                .InclusiveBetween(1, 31).WithMessage("El día de cierre debe estar entre 1 y 31.");

        }
    }
}
