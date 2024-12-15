using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("PaymentClaims")]
    public class PaymentClaim:Claim
    {
        [Required]
        [Column(TypeName ="decimal(18,2)")]
        [Range(0.01, 1000000.00, ErrorMessage = "Значение должно быть от 0.01 до 1,000,000.00.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal ClaimAmount {  get; set; }
        [Required]
        [StringLength(100)]
        public string ClientBankAccount { get; set; }
        [Required]
        [ForeignKey("Policy")]
        public int PolicyId {  get; set; }
        [ValidateNever]
        public Policy Policy { get; set; }
        [Required]
        [ForeignKey("Client")]
        public int ClientId {  get; set; }
        [ValidateNever]
        public Client Client { get; set; }
    }
}
