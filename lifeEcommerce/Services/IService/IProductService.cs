using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Dtos.Product;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Services.IService
{
    public interface IProductService
    {
        Task CreateProduct(ProductCreateDto productToCreate);
        Task DeleteProduct(int id);
        Task<List<Product>> GetAllProducts();
        Task<PagedInfo<Product>> ProductsListView(string search, int page, int pageSize, int categoryId);
        Task<Product> GetProduct(int id);
        Task UpdateProduct(ProductDto productToUpdate);
        Task<Product> GetWithIncludes(int id);
    }
}
