using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("Clients")]
    public class Client:User
    {
        public ICollection<PolicyClaim>? PolicyClaims { get; set; }
    }
}
