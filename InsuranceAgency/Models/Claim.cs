using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsuranceAgency.Models
{
    public abstract class Claim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly ClaimDate { get; set; }
        [Required]
        public ClaimStatus ClaimStatus { get; set; }
    }
    public enum ClaimStatus
    {
        Создана = 1,
        Обрабатывается = 2,
        Одобрена = 3,
        Отклонена = 4
    }
}
