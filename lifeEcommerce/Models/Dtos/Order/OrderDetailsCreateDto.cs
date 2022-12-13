using System.ComponentModel.DataAnnotations.Schema;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Models.Dtos.Order
{
    public class OrderDetailsCreateDto
    {
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public OrderData OrderData { get; set; }

        public int ProductId { get; set; }
        public Entities.Product Product { get; set; }

        public int Count { get; set; }

        public double Price { get; set; }
    }
}
