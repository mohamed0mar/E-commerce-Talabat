using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;

namespace E_Commerce.Core
{
	public interface IUnitOfWork:IAsyncDisposable
	{
		IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

		Task<int> CompleteAsync();
	}
}
