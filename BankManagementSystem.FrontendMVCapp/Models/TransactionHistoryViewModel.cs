namespace BankManagementSystem.FrontendMVCapp.Models
{
    public class TransactionHistoryViewModel
    {
        public int TransactionHistoryId { get; set; }

        public string FromAccountNo { get; set; }

        public int FromAccountId { get; set; }

        public string ToAccountNo { get; set; }

        public int ToAccountId { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }
    }
}
