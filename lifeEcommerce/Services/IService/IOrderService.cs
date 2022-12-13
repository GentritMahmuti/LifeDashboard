namespace lifeEcommerce.Services.IService
{
    public interface IOrderService
    {
        Task ProcessOrder(List<string> orderIds, string status);
    }
}
