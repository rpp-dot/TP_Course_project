using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("PolicyClaims")]
    public class PolicyClaim:Claim
    {
        [ForeignKey("Service")]
        public int ServiceId {  get; set; }
        [ForeignKey("InsuranceObject")]
        public int InsuranceObjectId { get; set; }
        [ForeignKey("Client")]
        public int ClientId { get; set; }
    }
}
