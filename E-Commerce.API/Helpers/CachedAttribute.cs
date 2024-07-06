using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Helpers
{
	public class CachedAttribute : Attribute, IAsyncActionFilter
	{
		private readonly int _timeToLiveInSeconde;

		public CachedAttribute(int TimeToLiveInSeconde)
		{
			_timeToLiveInSeconde = TimeToLiveInSeconde;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			//Ask CLR to create object from IResponseService Exceplicitly
			var responseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseService>();

			var cacheKey = GeneretCacheKeyFromRequest(context.HttpContext.Request);

			var response = await responseCacheService.GetCacheResponse(cacheKey);

			if (!string.IsNullOrEmpty(response))
			{
				var result = new ContentResult()
				{
					Content = response,
					ContentType = "application/json",
					StatusCode = 200
				};

				context.Result = result;
				return;
			} //Response Is Not Cached

			var executedActionContext= await next.Invoke();
			if (executedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
			{
				await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconde));
			}
		}

		private string GeneretCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();
			keyBuilder.Append(request.Path);
			foreach (var (key,value) in request.Query.OrderBy(X=>X.Key))
			{
				keyBuilder.Append($"|{key}-{value}");
			}
			return keyBuilder.ToString();
		}
	}
}
