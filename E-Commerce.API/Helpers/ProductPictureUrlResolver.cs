using AutoMapper;
using Castle.Core.Internal;
using E_Commerce.API.Dtos.Product;
using E_Commerce.Core.Entities.Product;

namespace E_Commerce.API.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
	{
		private readonly IConfiguration _configuration;

		public ProductPictureUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(Product source, ProductToReturnDto destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.PictureUrl))
				return $"{_configuration["ApiBaseUrl"]}/{source.PictureUrl}";
			return string.Empty ;
		}
	}
}
