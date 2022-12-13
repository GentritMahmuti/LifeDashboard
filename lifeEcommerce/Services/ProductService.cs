using lifeEcommerce.Models.Entities;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Helpers;
using AutoMapper;
using lifeEcommerce.Models.Dtos.Product;

namespace lifeEcommerce.Services
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var categories = _unitOfWork.Repository<Product>().GetAll();

            return categories.ToList();
        }

        public async Task<Product> GetProduct(int id)
        {
            Expression<Func<Product, bool>> expression = x => x.Id == id;
            var product = await _unitOfWork.Repository<Product>().GetById(expression).FirstOrDefaultAsync();

            return product;
        }

        //public async Task<Product> GetWithInculdes(int id)
        //{
        //    Expression<Func<Product, bool>> expression = x => x.Id == id;
        //    var product = await _unitOfWork.Repository<Product>().GetByConditionWithIncludes(expression, "Category, Unit").FirstOrDefaultAsync();

        //    return product;
        //}

        public async Task UpdateProduct(ProductDto productToUpdate)
        {
            Product? product = await GetProduct(productToUpdate.Id);

            product.Title = productToUpdate.Title;

            _unitOfWork.Repository<Product>().Update(product);

            _unitOfWork.Complete();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await GetProduct(id);

            _unitOfWork.Repository<Product>().Delete(product);

            _unitOfWork.Complete();
        }

        public async Task CreateProduct(ProductCreateDto coverToCreate)
        {
            var product = _mapper.Map<Product>(coverToCreate);

            _unitOfWork.Repository<Product>().Create(product);

            _unitOfWork.Complete();
        }

        public async Task<PagedInfo<Product>> ProductsListView(string search, int page, int pageSize, int categoryId = 0)
        {
            Expression<Func<Product, bool>> condition = x => x.Title.Contains(search);

            IQueryable<Product> products;


            if (categoryId is not 0)
            {
                Expression<Func<Product, bool>> conditionByCategory = x => x.CategoryId == categoryId;
                products = _unitOfWork.Repository<Product>()
                                             .GetByCondition(conditionByCategory)
                                             .WhereIf(!string.IsNullOrEmpty(search), condition)
                                             .PageBy(x => x.Id, page, pageSize);
            }
            else // dismiss category
            {
                products = _unitOfWork.Repository<Product>().GetAll().WhereIf(!string.IsNullOrEmpty(search), condition);
            }   
                
            var count = await products.CountAsync();

            var categoriesPaged = new PagedInfo<Product>()
            {
                TotalCount = count,
                Page = page,
                PageSize = pageSize,
                Data = await products
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize).ToListAsync()
            };

            return categoriesPaged;
        }

        public async Task<Product> GetWithIncludes(int id)
        {
            Expression<Func<Product, bool>> expression = x => x.Id == id;
            var product = await _unitOfWork.Repository<Product>().GetByConditionWithIncludes(expression, "Category, Unit").FirstOrDefaultAsync();

            return product;
        }
    }
}
