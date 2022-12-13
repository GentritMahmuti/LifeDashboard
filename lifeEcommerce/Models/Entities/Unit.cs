using System.ComponentModel.DataAnnotations;

namespace lifeEcommerce.Models.Entities
{
    public class Unit
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
