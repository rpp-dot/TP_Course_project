using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace InsuranceAgency.Models
{
    [Table("Clients")]
    public class Client:User
    {
        public ICollection<PolicyClaim>? PolicyClaims { get; set; }
        public ICollection<PaymentClaim>? PaymentClaims { get; set; }
        public ICollection<Policy>? Policies { get; set; }
    }
}
