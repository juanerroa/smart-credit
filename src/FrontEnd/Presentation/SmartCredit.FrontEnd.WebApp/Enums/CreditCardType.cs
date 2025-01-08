using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SmartCredit.FrontEnd.WebApp.Enums
{
    public enum CreditCardType
    {
        [Display(Name = "Visa")]
        Visa = 1,
        [Display(Name = "MasterCard")]
        MasterCard = 2,
        [Display(Name = "American Express")]
        AmericanExpress = 3,
        [Display(Name = "Desconocida")]
        Desconocida = 4
    }
}