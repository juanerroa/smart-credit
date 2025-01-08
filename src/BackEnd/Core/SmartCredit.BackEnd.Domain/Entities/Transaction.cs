using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Base;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Domain.Entities
{
    public class Transaction : BaseEntity<Guid>
    {
        public Guid CreditCardId { get; set; } // Relación con la tarjeta de crédito
        public string Description { get; set; } // Descripción de la compra o pago
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; } // Monto de la transacción
        public TransactionType Type { get; set; } // Tipo: Compra o Pago


        //Navigation relationships
        public CreditCard CreditCard { get; set; } // Propiedad de navegación
        public DateTime Date { get; set; } // Fecha de la transacción
    }
}