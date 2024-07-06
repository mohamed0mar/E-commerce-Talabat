using E_Commerce.Core;
using E_Commerce.Core.Entities.Product;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Core.Specifications.ProductSpecs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Service.Product_Service
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductService(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
		}

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(specParams);
			var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
			return products;
		}

		public async Task<int> GetCountAsync(ProductSpecParams specParams)
		{

			var countSpec = new ProductWithFilterationForCountSpecificarions(specParams);
			var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);
			return count;
		}

		public async Task<Product?> GetProductAsync(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications(id);
			var product = await _unitOfWork.Repository<Product>().GetWithSpecAsync(spec);
			return product;
		}

		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
			 =>await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

		public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
			=> await _unitOfWork.Repository<ProductCategory>().GetAllAsync();



	}
}
