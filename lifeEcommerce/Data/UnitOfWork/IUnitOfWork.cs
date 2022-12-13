using lifeEcommerce.Data.Repository.IRepository;

namespace lifeEcommerce.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IEcommerceRepository<TEntity> Repository<TEntity>() where TEntity : class;
        bool Complete();
    }
}
