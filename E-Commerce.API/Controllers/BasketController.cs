using AutoMapper;
using E_Commerce.API.Dtos.Basket;
using E_Commerce.API.Errors;
using E_Commerce.API.Helpers;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Repositories.Contract;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IMapper _mapper;

		public BasketController(IBasketRepository basketRepository,IMapper mapper)
		{
			_basketRepository = basketRepository;
			_mapper = mapper;
		}


		[Cached(600)]
		[HttpGet] //GET : /api/basket  
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepository.GetBasketAsync(id);

			return Ok(basket is null ? new CustomerBasket(id) : basket);
		}

		
		[HttpPost] //Post : /api/basket
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
		{
			var mappedBasket=_mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
			var createdOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);
			if (createdOrUpdatedBasket is null)
				return BadRequest(new ApiResponse(400));
			return Ok(createdOrUpdatedBasket);
		}

		
		[HttpDelete] //Delete : /api/basket
		public async Task<ActionResult<bool>> DeleteBasket(string id)
		{
			return await _basketRepository.DeleteBasketAsync(id);
		}

	}
}
