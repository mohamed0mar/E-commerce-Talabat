﻿using E_Commerce.Core.Entities;
using E_Commerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Repositories.Contract
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<T?> GetAsync(int id);
		Task<IReadOnlyList<T>> GetAllAsync();

		Task<T?> GetWithSpecAsync(ISpecifications<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

		Task<int> GetCountAsync(ISpecifications<T> spec);

		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);
	}
}
