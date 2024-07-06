using E_Commerce.Core.Entities;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Specifications;
using E_Commerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbcontext;

		public GenericRepository(StoreContext dbcontext)
        {
			_dbcontext = dbcontext;
		}
        public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbcontext.Set<T>().ToListAsync();
		}
		public async Task<T?> GetAsync(int id)
		{
			return await _dbcontext.Set<T>().FindAsync(id);
		}


		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		}

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();
		}

		public async Task<int> GetCountAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).CountAsync();
		}
		public IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
		{
			return SpecificationsEvaluator<T>.GetQuery(_dbcontext.Set<T>(), spec);
		}

		public void Add(T entity)
			=>_dbcontext.Set<T>().Add(entity);

		public void Update(T entity)
			=>_dbcontext.Set<T>().Update(entity);

		public void Delete(T entity)
			=>_dbcontext.Set<T>().Remove(entity);



		//_dbContext.Set<Product>().where(P=>P.Id==id)Include(P=>P.Brand).Include(P=>P.Category)
	}
}
