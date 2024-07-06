using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Repositories.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Basket_Repository
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer Redis)
        {
            _database=Redis.GetDatabase();
        }
       

		public async Task<CustomerBasket?> GetBasketAsync(string id)
		{
			var basket=await _database.StringGetAsync(id);

			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
		{
			var createdOrUpdatedBasket = await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
			if (!createdOrUpdatedBasket)
				return null;
			return await GetBasketAsync(basket.Id);
		}

		public async Task<bool> DeleteBasketAsync(string id)
		{
			return await _database.KeyDeleteAsync(id);
		}
	}
}
