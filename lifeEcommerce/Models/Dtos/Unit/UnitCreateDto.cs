using System.ComponentModel.DataAnnotations;

namespace lifeEcommerce.Models.Dtos.Unit
{
    public class UnitCreateDto
    {
        [Display(Name = "Cover Type")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
