using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("PolicyClaims")]
    public class PolicyClaim:Claim
    {
        [Required]
        [ForeignKey("Service")]
        public int ServiceId {  get; set; }
        [Required]
        [ForeignKey("InsuranceObject")]
        public int InsuranceObjectId { get; set; }
        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
    }
}
