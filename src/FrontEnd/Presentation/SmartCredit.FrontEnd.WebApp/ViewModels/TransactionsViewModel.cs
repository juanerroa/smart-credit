using System.ComponentModel.DataAnnotations;

namespace SmartCredit.FrontEnd.WebApp.ViewModels
{
    public class TransactionsViewModel
    {
        public Guid Id { get; set; } // Id
        public string Description { get; set; } // Descripción de la compra o pago
        public decimal Amount { get; set; } // Monto de la transacción
        public string TypeName { get; set; } // Tipo (Nombre)
        public DateTime Date { get; set; } //Fecha de transaccion
        public Guid CreditCardId { get; set; } // Relación con la tarjeta de crédito
        public int Type { get; set; } // Tipo
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
