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
        public decimal Price {  get; set; }
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
    }
}
