using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services.Contract
{
	public interface IResponseService
	{
		Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);
		Task<string?> GetCacheResponse(string key);
	}
}
