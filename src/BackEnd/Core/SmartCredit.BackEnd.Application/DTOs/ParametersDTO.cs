using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.DTOs
{
    public class ParametersDTO
    {
        public decimal InterestRate { get; set; } // Porcentaje de interés configurable
        public decimal MinimumPaymentPercentage { get; set; } // Porcentaje de pago mínimo configurable
    }
}
