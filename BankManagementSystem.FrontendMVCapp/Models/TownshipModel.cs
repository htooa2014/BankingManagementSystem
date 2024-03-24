using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.FrontendMVCapp.Models
{
    [Table("Township")]
    public class TownshipModel
    {
        [Key]

        public int TownshipId { get; set; }

        public string TownshipCode { get; set; }

        public string TownshipName { get; set; }

        public string StateCode { get; set; }
    }
}
