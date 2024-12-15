using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    [Table("InsuranceObjects")]
    public class InsuranceObject
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000.00, ErrorMessage = "Значение должно быть от 0.01 до 1,000,000.00.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Price {  get; set; }
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
    }
}
