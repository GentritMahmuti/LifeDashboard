using lifeEcommerce.Models.Dtos.Order;
using lifeEcommerce.Models.Dtos.ShoppingCard;

namespace lifeEcommerce.Services.IService
{
    public interface IShoppingCardService
    {
        Task AddProductToCard(string userId, int productId, int count);
        Task<ShoppingCardDetails> GetShoppingCardContentForUser(string userId);
        Task Plus(int shoppingCardItemId, int? newQuantity);
        Task Minus(int shoppingCardItemId, int? newQuantity);
        Task CreateOrder(AddressDetails addressDetails, List<ShoppingCardViewDto> shoppingCardItems);
    }
}
