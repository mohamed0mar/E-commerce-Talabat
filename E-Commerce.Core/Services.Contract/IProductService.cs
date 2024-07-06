using E_Commerce.Core.Entities.Product;
using E_Commerce.Core.Specifications.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services.Contract
{
	public interface IProductService
	{
		Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);

		Task<int> GetCountAsync(ProductSpecParams specParams);
		Task<Product?> GetProductAsync(int id);

		Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
		Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync();

	}
}
