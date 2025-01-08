using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.DTOs
{
    public class CreditCardDTO
    {
        public Guid Id { get; set; } //UID de la tarjeta
        public string CardNumber { get; set; } // Número de la tarjeta
        public string HolderName { get; set; } // Número de la tarjeta
        public decimal CreditLimit { get; set; } // Límite de crédito
        public decimal Balance { get; set; } // Saldo actual
        public decimal AvailableBalance { get; set; } // Saldo disponible
        public int ClosingDay  { get; set; } // Fecha de corte de período
        public Guid UserId { get; set; } // Relación con el usuario
        public int Type { get; set; } // Tipo de tarjeta
        public string TypeName { get; set; } // Tipo de tarjeta (Nombre)
        public decimal ConfigurableInterestRate { get; set; } // Porcentaje de interes configurable
        public decimal ConfigurableMinimumBalanceRate { get; set; } // Porcentaje configurable de saldo minimo
    }
}