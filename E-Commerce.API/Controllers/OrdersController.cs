using AutoMapper;
using E_Commerce.API.Dtos.Order;
using E_Commerce.API.Errors;
using E_Commerce.API.Helpers;
using E_Commerce.Core.Entities.Order_Aggregate;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
	[Authorize]
	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(
			IOrderService orderService,
			IMapper mapper
			)
		{
			_orderService = orderService;
			_mapper = mapper;
		}

		
		
		[HttpPost] //POST : /api/orders
		[ProducesResponseType( typeof( OrderToReturnDto ), StatusCodes.Status200OK )]
		[ProducesResponseType( typeof(ApiResponse), StatusCodes.Status400BadRequest )]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<OrderAddressDto, OrderAddress>(orderDto.ShippingAddress);
			var email = User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.CreateOrderAsync(email, orderDto.BasketId, orderDto.DeliveryMethodId, address);
			if (order is null)
				return BadRequest(new ApiResponse(400));
			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}


		[Cached(600)]
		[HttpGet] //GET :/api/orders
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var orders = await _orderService.GetOrdersForUserAsync(email);
			return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
		}


	
		[HttpGet("{id}")] //GET : /api/orders/4
		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<Order>> GetOrderForUser(int id)
		{
			var email=User.FindFirstValue(ClaimTypes.Email);
			var order = await _orderService.GetOrderByIdForUserAsync(email, id);
			if(order is null)
				return NotFound(new ApiResponse(404));
			return Ok(_mapper.Map<OrderToReturnDto>(order));
		}

		
		[HttpGet("deliverymethods")] //GET : /api/orders/deliverymethods
		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveryMethods=await _orderService.GetDeliveryMethodsAsync();
			return Ok(deliveryMethods);
		}
	}
}
