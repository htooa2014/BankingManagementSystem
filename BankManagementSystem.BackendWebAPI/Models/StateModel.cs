using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.BackendWebAPI.Models
{
    [Table("State")]
    public class StateModel
    {
        [Key]

        public int StateId { get; set; }

        public string StateCode { get; set; }

        public string StateName { get; set; }
        
    }
}
