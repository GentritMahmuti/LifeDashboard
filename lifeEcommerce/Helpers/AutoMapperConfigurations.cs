using AutoMapper;
using lifeEcommerce.Models.Dtos.Category;
using lifeEcommerce.Models.Dtos.Product;
using lifeEcommerce.Models.Dtos.Unit;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();

            CreateMap<Unit, UnitDto>().ReverseMap();
            CreateMap<Unit, UnitCreateDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
        }
    }
}
