using E_Commerce.Core;
using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Repository.Data;
using System.Collections;

namespace E_Commerce.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;
		private Hashtable _repositories;
		public UnitOfWork(StoreContext dbContext)
        {
			_dbContext = dbContext;
			_repositories=new Hashtable();
		}
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var key=typeof(TEntity).Name; 
			if (!_repositories.ContainsKey(key))
			{
				var repository = new GenericRepository<TEntity>(_dbContext);
				_repositories.Add(key, repository);
			}
			return _repositories[key]as IGenericRepository<TEntity>;
		}

		public async Task<int> CompleteAsync()
			=>await _dbContext.SaveChangesAsync();

		public async ValueTask DisposeAsync()
			=>await _dbContext.DisposeAsync();

		
	}
}
