using AutoMapper;
using E_Commerce.API.Dtos.Product;
using E_Commerce.API.Errors;
using E_Commerce.API.Helpers;
using E_Commerce.Core.Entities.Product;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Core.Specifications.ProductSpecs;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

	public class ProductsController : BaseApiController
	{
		private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public ProductsController(
			IProductService productService,
			IMapper mapper
			)
		{
			_productService = productService;
			_mapper = mapper;
		}


		[Cached(600)]  //Action Filter
		[HttpGet] //GET //api/Products
		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams specParams)
		{
			var products = await _productService.GetProductsAsync(specParams);
			var count = await _productService.GetCountAsync(specParams);

			var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex,specParams.PageSize, count, data));

		}
		
		
		[Cached(600)]
		[HttpGet("{id}")] //GET //api/Products/10
		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			
			var product = await _productService.GetProductAsync(id);
			if (product is null)
				return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
		}


		
		[HttpGet("brands")] //GET :/api/products/brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands=await _productService.GetBrandsAsync();
			return Ok(brands);
		}


		
		[HttpGet("categories")] //GET :/api/products/categories
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories=await _productService.GetCategoriesAsync();
			return Ok(categories);
		}
	}
}
