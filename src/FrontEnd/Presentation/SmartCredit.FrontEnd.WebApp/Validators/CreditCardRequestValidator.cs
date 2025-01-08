using FluentValidation;
using SmartCredit.FrontEnd.WebApp.ViewModels;

namespace SmartCredit.FrontEnd.WebApp.Validators
{
    public class CreditCardRequestValidator : AbstractValidator<CreditCardViewModel>
    {
        public CreditCardRequestValidator()
        {
            // Validación para FullName
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("El nombre completo es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre completo no debe exceder los 100 caracteres.");

            // Validación para Address
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no debe exceder los 200 caracteres.");

            // Validación para City
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("La ciudad es obligatoria.")
                .MaximumLength(100).WithMessage("La ciudad no debe exceder los 100 caracteres.");

            // Validación para State
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("El estado es obligatorio.")
                .MaximumLength(100).WithMessage("El estado no debe exceder los 100 caracteres.");

            // Validación para Country
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("El país es obligatorio.")
                .MaximumLength(100).WithMessage("El país no debe exceder los 100 caracteres.");

            // Validación para Email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("Debe proporcionar un correo electrónico válido.");

            // Validación para CardNumber
            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("El número de tarjeta es obligatorio.")
                .CreditCard().WithMessage("Debe proporcionar un número de tarjeta de crédito válido.");

            // Validación para HolderName
            RuleFor(x => x.HolderName)
                .NotEmpty().WithMessage("El nombre del titular es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre del titular no debe exceder los 100 caracteres.");

            // Validación para CreditLimit
            RuleFor(x => x.CreditLimit)
                .GreaterThan(0).WithMessage("El límite de crédito debe ser mayor a 0.");

            // Validación para ClosingDay
            RuleFor(x => x.ClosingDay)
                .InclusiveBetween(1, 31).WithMessage("El día de cierre debe estar entre 1 y 31.");
        }
    }
}
