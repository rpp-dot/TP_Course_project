using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("Policies")]
    public class Policy
    {
        [Key] public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly StartDate { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly EndDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PremiumAmount {  get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaymentCoef { get; set; }
        [Required]
        public PolicyStatus Status { get; set; }
        [Required]
        [ForeignKey("InsuranceObject")]
        public int InsuranceObjectId { get; set; }
        [Required]
        [ForeignKey("InsuranceAgent")]
        public int InsuranceAgentId { get; set; }
        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }

    }
    public enum PolicyStatus
    {
        Активен = 1,
        Истек = 2,
        Подозрительный = 3,
        Аннулирован = 4
    }
}
