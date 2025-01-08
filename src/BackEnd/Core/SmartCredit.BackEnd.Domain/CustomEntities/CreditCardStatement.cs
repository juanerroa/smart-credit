using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Domain.CustomEntities
{
    public class CreditCardStatement
    {
        public string HolderName { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public decimal CreditLimit { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal TotalPurchaseSelectedPeriod { get; set; }
        public decimal TotalPaymentsSelectedPeriod { get; set; }
        public decimal TotalPurchaseLastPeriod { get; set; }
        public decimal TotalPaymentsLastPeriod { get; set; }
        public decimal BonusInterest { get; set; }
        public decimal MinimumQuota { get; set; }
        public decimal TotalAmountWithInterest { get; set; }
        public decimal TotalPeriodBalance { get; set; }
    }
}
