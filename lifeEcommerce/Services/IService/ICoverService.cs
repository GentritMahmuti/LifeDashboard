using lifeEcommerce.Models.Dtos.Unit;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Services.IService
{
    public interface ICoverService
    {
        Task CreateCover(UnitCreateDto coverToCreate);
        Task<List<Unit>> GetAllCovers();
        Task<Unit> GetCover(int id);
        Task UpdateCover(UnitDto coverToUpdate);
        Task DeleteCover(int id);
    }
}
