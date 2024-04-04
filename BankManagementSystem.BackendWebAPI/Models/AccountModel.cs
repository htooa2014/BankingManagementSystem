using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.BackendWebAPI.Models
{
    [Table("Account")]
    public class AccountModel
    {
        [Key]

        public int AccountId { get; set; }

        public string AccountNo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public decimal Balance { get; set; }

        //0 - Normal
        //1 - Deposit
        //2 - Withdraw
        public int TransactionStatus { get; set; }
    }
}
