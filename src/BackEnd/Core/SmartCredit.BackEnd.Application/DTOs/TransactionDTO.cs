using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Application.DTOs
{
    public class TransactionDTO
    {
        public Guid Id { get; set; } // Id
        public Guid CreditCardId { get; set; } // Relación con la tarjeta de crédito
        public string Description { get; set; } // Descripción de la compra o pago
        public decimal Amount { get; set; } // Monto de la transacción
        public int Type { get; set; } // Tipo
        public string TypeName { get; set; } // Tipo (Nombre)
        public DateTime Date { get; set; } //Fecha de transaccion

    }
}