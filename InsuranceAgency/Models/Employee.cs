using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    public abstract class Employee:User
    {
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary {  get; set; }
    }
}
