using System.ComponentModel.DataAnnotations;

namespace InsuranceAgency.Models
{
    public abstract class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Login {  get; set; }
        [Required]
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname {  get; set; }
        [Required,StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Patronymic { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(30)]
        public string Role { get; set; }
    }
}
