using lifeEcommerce.Models.Entities;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Models.Dtos.Unit;

namespace lifeEcommerce.Services
{
    public class CoverService : ICoverService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCover(UnitCreateDto coverToCreate)
        {
            var cover = new Unit
            {
                Name = coverToCreate.Name
            };

            _unitOfWork.Repository<Unit>().Create(cover);

            _unitOfWork.Complete();
        }

        public async Task<List<Unit>> GetAllCovers()
        {
            var covers = _unitOfWork.Repository<Unit>().GetAll().ToList();

            return covers.ToList();
        }

        public async Task<Unit> GetCover(int id)
        {
            Expression<Func<Unit, bool>> expression = x => x.Id == id;
            var cover = await _unitOfWork.Repository<Unit>().GetById(expression).FirstOrDefaultAsync();

            return cover;
        }

        public async Task UpdateCover(UnitDto coverToUpdate)
        {
            var cover = await GetCover(coverToUpdate.Id);

            cover.Name = coverToUpdate.Name;

            _unitOfWork.Repository<Unit>().Update(cover);

            _unitOfWork.Complete();
        }

        public async Task DeleteCover(int id)
        {
            var cover = await GetCover(id);

            _unitOfWork.Repository<Unit>().Delete(cover);

            _unitOfWork.Complete();
        }
    }
}
