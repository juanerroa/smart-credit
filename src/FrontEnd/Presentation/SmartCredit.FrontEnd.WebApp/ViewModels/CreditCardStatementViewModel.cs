namespace SmartCredit.FrontEnd.WebApp.ViewModels
{
    public class CreditCardStatementViewModel
    {
        public CreditCardStatementViewModel() 
        {
            Transactions = new List<TransactionsViewModel>();
        }

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
        public List<TransactionsViewModel> Transactions { get; set; }


        public int Year { get; set; }
        public int Month { get; set; }
    }
}
