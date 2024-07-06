using E_Commerce.Core.Services.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Service.Cache_Service
{
	public class ResponseService : IResponseService
	{
		private readonly IDatabase _database;

		public ResponseService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}

		public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
		{
			if (response is null)
				return;
			var serializedOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var serializedRespons = JsonSerializer.Serialize(response, serializedOptions);

			await _database.StringSetAsync(key, serializedRespons, timeToLive);


		}

		public async Task<string?> GetCacheResponse(string key)
		{
			var response = await _database.StringGetAsync(key);

			if (response.IsNullOrEmpty)
				return null;
			return response;
		}
	}
}
