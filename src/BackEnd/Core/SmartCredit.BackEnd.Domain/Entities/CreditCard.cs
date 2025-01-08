using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Base;
using SmartCredit.BackEnd.Domain.Enums;

namespace SmartCredit.BackEnd.Domain.Entities
{
    public class CreditCard : BaseEntity<Guid>
    {
        public string CardNumber { get; set; } // Número de la tarjeta
        public string HolderName { get; set; } // Titular de la tarjeta

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CreditLimit { get; set; } // Límite de crédito

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; } // Saldo actual

        [Column(TypeName = "decimal(18, 2)")]
        public decimal AvailableBalance { get; set; } // Saldo disponible
        public int ClosingDay  { get; set; } // Fecha de corte de período
        public Guid UserId { get; set; } // Relación con el usuario
        public CreditCardType Type { get; set; } // Tipo de tarjeta

        [Column(TypeName = "decimal(5, 2)")]
        public decimal ConfigurableInterestRate { get; set; } // Porcentaje de interes configurable

        [Column(TypeName = "decimal(5, 2)")]
        public decimal ConfigurableMinimumBalanceRate { get; set; } // Porcentaje configurable de saldo minimo

        //Navigation relationships
        public User User { get; set; } // Propiedad de navegación (para la relación)
    }
}