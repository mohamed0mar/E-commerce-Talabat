using E_Commerce.API.Errors;
using E_Commerce.Core.Entities.Basket;
using E_Commerce.Core.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace E_Commerce.API.Controllers
{

	public class PaymentController : BaseApiController
	{
		private readonly IPaymentService _paymentService;
		// This is your Stripe CLI webhook secret for testing your endpoint locally.
		private const string whSecret = "whsec_eb4f71b1dc2db93e2873e4b75c52617d431e186499b9fe66717d4ef0d63f441d";

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}


		[ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost("{basketId}")] //POST : /api/payment/{basketId}
		public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
		{
			var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
			if (basket is null)
				return BadRequest(new ApiResponse(400, "An Error With Your Basket"));
			return Ok(basket);
		}

		[HttpPost("webhook")]
		public async Task<IActionResult> WebHook()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

			var stripeEvent = EventUtility.ConstructEvent(json,
				Request.Headers["Stripe-Signature"], whSecret);

			var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

			// Handle the event
			switch (stripeEvent.Type)
			{
				case Events.PaymentIntentSucceeded:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
					break;
				case Events.PaymentIntentPaymentFailed:
					await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
					break;
			}

			return Ok();

		}
	}
}
