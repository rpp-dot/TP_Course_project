using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("Services")]
    public class Service
    {
        [Key] public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string InsuranceObjectType { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumCoef {  get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentCoef { get; set;}
    }
}
