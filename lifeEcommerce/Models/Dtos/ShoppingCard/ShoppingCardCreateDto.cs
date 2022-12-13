using lifeEcommerce.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace lifeEcommerce.Models.Dtos.ShoppingCard
{
    public class ShoppingCardDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Entities.Product Product { get; set; }
        public int Count { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
