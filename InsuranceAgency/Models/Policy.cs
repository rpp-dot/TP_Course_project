using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
        public DateTime StartDate { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateTime EndDate { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000.00, ErrorMessage = "Значение должно быть от 0.01 до 1,000,000.00.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal PremiumAmount {  get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000.00, ErrorMessage = "Значение должно быть от 0.01 до 1,000,000.00.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal PaymentCoef { get; set; }
        [Required]
        public PolicyStatus Status { get; set; }
        [Required]
        [ForeignKey("InsuranceObject")]
        public int InsuranceObjectId { get; set; }
        [ValidateNever]
        public InsuranceObject InsuranceObject { get; set; }
        [Required]
        [ForeignKey("InsuranceAgent")]
        public int InsuranceAgentId { get; set; }
        [ValidateNever]
        public InsuranceAgent InsuranceAgent { get; set; }
        [Required]
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [ValidateNever]
        public Client Client { get; set; }

    }
    public enum PolicyStatus
    {
        Активен = 1,
        Истек = 2,
        Подозрительный = 3,
        Аннулирован = 4
    }
}
