using FluentValidation;
using SmartCredit.FrontEnd.WebApp.ViewModels;

namespace SmartCredit.FrontEnd.WebApp.Validators
{
    public class TransactionsRequestValidator : AbstractValidator<TransactionsViewModel>
    {
        public TransactionsRequestValidator()
        {
            RuleFor(x => x.CreditCardId)
                .NotEmpty().WithMessage("El ID de la tarjeta de crédito no puede estar vacío.")
                .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("El ID de la tarjeta debe ser un GUID válido.");

            RuleFor(x => x.Type)
                .InclusiveBetween(1, 2).WithMessage("El tipo debe ser 1 (Compra) o 2 (Pago).");

            RuleFor(x => x.Date)
                .Must(date => date.Year >= DateTime.Now.Year)
                .WithMessage("El año debe ser debe coincidir con el actual.");

            // No debe ser nulo ni vacío
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción no puede estar vacía.");

            // Quiero que sea mayor a 0
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("El monto debe ser mayor a 0.");

        }
    }
}
