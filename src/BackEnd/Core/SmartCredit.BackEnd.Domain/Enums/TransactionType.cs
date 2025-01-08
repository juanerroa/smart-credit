using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SmartCredit.BackEnd.Domain.Enums
{
    public enum TransactionType
    {
        [Display(Name = "Purchase")]
        Purchase = 1,
        [Display(Name = "Payment")]
        Payment = 2
    }
}