namespace lifeEcommerce.Models.Dtos.ShoppingCard
{
    public class ShoppingCardDetails
    {
        public double CardTotal { get; set; }

        public List<ShoppingCardViewDto> ShoppingCardItems { get; set; }
    }
}
