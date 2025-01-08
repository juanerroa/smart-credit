using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.FrontEnd.WebApp.ViewModels
{
    public class PaymentViewModel
    {
        public decimal MinimumPayment {get; set;}
        public decimal TotalPaymentWithInterest {get; set;}
    }
}