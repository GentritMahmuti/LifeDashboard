using lifeEcommerce.Models.Dtos.Order;

namespace lifeEcommerce.Models.Dtos.ShoppingCard
{
    public class ProductSummaryModel
    {
        public AddressDetails AddressDetails { get; set; } 
            
        public List<ShoppingCardViewDto> ShoppingCardItems { get; set; }
    }
}