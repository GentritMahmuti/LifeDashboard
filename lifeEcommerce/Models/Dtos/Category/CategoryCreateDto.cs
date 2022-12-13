using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace lifeEcommerce.Models.Dtos.Category
{
    public class CategoryCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
