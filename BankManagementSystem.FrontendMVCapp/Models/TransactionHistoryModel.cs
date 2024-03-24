using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.FrontendMVCapp.Models
{
    [Table("TransactionHistory")]
    public class TransactionHistoryModel
    {
        [Key]

        public int TransactionHistoryId { get; set; }

        public string FromAccountNo { get; set; }

        public string ToAccountNo { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }
    }
}
