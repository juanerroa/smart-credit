using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.ViewModels
{
    public class CreditCardViewModel
    {
        [Display(Name = "UID de la tarjeta")]
        public Guid Id { get; set; }

        [Display(Name = "Número de la tarjeta")]
        public string CardNumber { get; set; }

        [Display(Name = "Titular de la tarjeta")]
        public string HolderName { get; set; }

        [Display(Name = "Límite de crédito")]
        public decimal CreditLimit { get; set; }

        [Display(Name = "Saldo actual")]
        public decimal Balance { get; set; }

        [Display(Name = "Saldo disponible")]
        public decimal AvailableBalance { get; set; }

        [Display(Name = "Fecha de corte de período")]
        public int ClosingDay { get; set; }

        [Display(Name = "Relación con el usuario")]
        public Guid UserId { get; set; }

        [Display(Name = "Tipo de tarjeta")]
        public int Type { get; set; }

        [Display(Name = "Nombre del tipo de tarjeta")]
        public string TypeName { get; set; }

        [Display(Name = "Nombre completo")]
        public string FullName { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Ciudad")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "País")]
        public string Country { get; set; }

        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Display(Name = "Porcentaje de interes")]
        public decimal ConfigurableInterestRate { get; set; }

        [Display(Name = "Porcentaje de saldo minimo")]
        public decimal ConfigurableMinimumBalanceRate { get; set; }

        public IEnumerable<KeyValuePair<int, string>> CreditCardTypeOptions { get; set; }
    }
}