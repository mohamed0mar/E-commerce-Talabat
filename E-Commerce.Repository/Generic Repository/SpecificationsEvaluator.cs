using E_Commerce.Core.Entities;
using E_Commerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
	public static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> spec)
		{
			var query = inputQuery;

			if(spec.Criteria is not null)
				query= query.Where(spec.Criteria);

			if(spec.OrderBy is not null)
				query= query.OrderBy(spec.OrderBy);

			else if(spec.OrderByDesc is not null) 
				query= query.OrderByDescending(spec.OrderByDesc);

			if(spec.IsPaginationEnabled)
				query=query.Skip(spec.Skip).Take(spec.Take);

			query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));


			return query;
		}
	}
}
