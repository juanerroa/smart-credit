using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Domain.CustomEntities
{
    public class Payment
    {
        public decimal MinimumPayment {get; set;}
        public decimal TotalPaymentWithInterest {get; set;}
    }
}