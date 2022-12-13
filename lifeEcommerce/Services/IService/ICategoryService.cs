using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Dtos.Category;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Services.IService
{
    public interface ICategoryService
    {
        Task CreateCategory(CategoryCreateDto categoryToCreate);
        Task DeleteCategory(int id);
        Task<List<Category>> GetAllCategories();
        Task<PagedInfo<Category>> CategoriesListView(string search, int page, int pageSize);
        Task<Category> GetCategory(int id);
        Task UpdateCategory(CategoryDto categoryToUpdate);
    }
}
