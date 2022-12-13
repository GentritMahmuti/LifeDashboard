using lifeEcommerce.Models.Entities;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Helpers;
using AutoMapper;
using lifeEcommerce.Models.Dtos.Category;

namespace lifeEcommerce.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            var categories = _unitOfWork.Repository<Category>().GetAll();

            return categories.ToList();
        }

        public async Task<Category> GetCategory(int id)
        {
            Expression<Func<Category, bool>> expression = x => x.Id == id;
            var category = await _unitOfWork.Repository<Category>().GetById(expression).FirstOrDefaultAsync();

            return category;
        }

        public async Task UpdateCategory(CategoryDto categoryToUpdate)
        {
            Category? category = await GetCategory(categoryToUpdate.Id);

            category.Name = categoryToUpdate.Name;
            category.DisplayOrder = categoryToUpdate.DisplayOrder;

            _unitOfWork.Repository<Category>().Update(category);

            _unitOfWork.Complete();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await GetCategory(id);

            _unitOfWork.Repository<Category>().Delete(category);

            _unitOfWork.Complete();
        }

        public async Task CreateCategory(CategoryCreateDto coverToCreate)
        {
            var category = _mapper.Map<Category>(coverToCreate);

            //var cover = new Cover
            //{
            //    Name = coverToCreate.Name
            //};

            _unitOfWork.Repository<Category>().Create(category);

            _unitOfWork.Complete();
        }

        public async Task<PagedInfo<Category>> CategoriesListView(string search, int page, int pageSize)
        {
            Expression<Func<Category, bool>> condition = x => x.Name.Contains(search);

            //var categories1 = _unitOfWork.Repository<Category>()
            //                                             .GetByConditionPaginated(condition, x => x.Id, page, pageSize, false);

            var categories = _unitOfWork.Repository<Category>()
                                                         .GetAll().WhereIf(!string.IsNullOrEmpty(search), condition);
                                                       
            var count = await categories.CountAsync();

            var categoriesPaged = new PagedInfo<Category>()
            {
                TotalCount = count,
                Page = page,
                PageSize = pageSize,
                Data = await categories
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync()
            };

            return categoriesPaged;
        }
    }
}
